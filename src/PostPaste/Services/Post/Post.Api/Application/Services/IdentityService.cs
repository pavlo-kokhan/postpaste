using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Options;
using Post.Api.Application.Services.Abstract;
using Post.Domain.Constants;
using Post.Domain.Entities.User;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IEmailService _emailService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly EmailUrlsOptions _emailUrlsOptions;

    public IdentityService(
        UserManager<UserEntity> userManager, 
        IEmailService emailService,
        IJwtTokenService jwtTokenService,
        IOptions<EmailUrlsOptions> emailConfirmationOptions)
    {
        _userManager = userManager;
        _emailService = emailService;
        _jwtTokenService = jwtTokenService;
        _emailUrlsOptions = emailConfirmationOptions.Value;
    }

    public async Task<Result> RegisterUserAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is not null)
            return Result.ValidationFailure(IdentityErrors.UserAlreadyExists);

        var userEntity = new UserEntity { Email = email, UserName = email};
        var createUserResult = await _userManager.CreateAsync(userEntity, password);

        if (!createUserResult.Succeeded)
            return Result.ValidationFailure(IdentityErrors.UserCreationFailed);

        var roleAssignmentResult = await _userManager.AddToRoleAsync(userEntity, nameof(Roles.User));
        
        if (!roleAssignmentResult.Succeeded)
            return Result.ValidationFailure(IdentityErrors.RoleAssignmentFailed);

        await _emailService.SendUserConfirmationEmailAsync(
            email, 
            userEntity.Id,
            await _userManager.GenerateEmailConfirmationTokenAsync(userEntity),
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ResendConfirmationEmailAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);
        
        if (userEntity is null)
            return Result.ValidationFailure(IdentityErrors.UserNotFound);
        
        if (!await _userManager.CheckPasswordAsync(userEntity, password))
            return Result.ValidationFailure(IdentityErrors.InvalidPassword);

        if (await _userManager.IsEmailConfirmedAsync(userEntity))
            return Result.ValidationFailure(IdentityErrors.EmailAlreadyConfirmed);

        await _emailService.SendUserConfirmationEmailAsync(
            email, 
            userEntity.Id,
            await _userManager.GenerateEmailConfirmationTokenAsync(userEntity),
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result<AccessTokenResponse>> LoginUserAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);
        
        if (userEntity is null)
            return Result<AccessTokenResponse>.ValidationFailure(IdentityErrors.UserNotFound);

        if (!await _userManager.IsEmailConfirmedAsync(userEntity))
            return Result<AccessTokenResponse>.ValidationFailure(IdentityErrors.EmailNotConfirmed);

        var signInResult = await _userManager.CheckPasswordAsync(userEntity, password);

        if (!signInResult)
            return Result<AccessTokenResponse>.ValidationFailure(IdentityErrors.InvalidPassword);

        return await _jwtTokenService.GenerateAccessTokenAsync(userEntity, cancellationToken);
    }

    public async Task<Result> ChangePasswordAsync(
        string email,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);
        
        if (userEntity is null)
            return Result.ValidationFailure(IdentityErrors.UserNotFound);
        
        if (!await _userManager.CheckPasswordAsync(userEntity, oldPassword))
            return Result.ValidationFailure(IdentityErrors.InvalidPassword);
        
        var result = await _userManager.ChangePasswordAsync(userEntity, oldPassword, newPassword);
        
        return !result.Succeeded 
            ? Result.ValidationFailure(IdentityErrors.PasswordChangeFailed) 
            : Result.Success();
    }

    public async Task<Result<string>> ConfirmUserEmailAsync(
        int id, 
        string token, 
        CancellationToken cancellationToken = default)
    {
        var userEntity = await _userManager.FindByIdAsync(id.ToString());
        
        if (userEntity is null)
            return Result<string>.ValidationFailure(IdentityErrors.UserNotFound);
        
        var decodedBytes = WebEncoders.Base64UrlDecode(token);
        var decodedToken = Encoding.UTF8.GetString(decodedBytes);
        
        var result = await _userManager.ConfirmEmailAsync(userEntity, decodedToken);
        
        return !result.Succeeded 
            ? Result<string>.ValidationFailure(IdentityErrors.UserConfirmationFailed) 
            : _emailUrlsOptions.LoginBaseUrl;
    }
}