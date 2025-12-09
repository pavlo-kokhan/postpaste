using Microsoft.AspNetCore.Authentication.BearerToken;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services.Abstract;

public interface IIdentityService
{
    public Task<Result> RegisterUserAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default);

    public Task<Result> ResendConfirmationEmailAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default);

    public Task<Result<AccessTokenResponse>> LoginUserAsync(
        string email, 
        string password, 
        CancellationToken cancellationToken = default);

    public Task<Result> ChangePasswordAsync(
        string email,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    public Task<Result<string>> ConfirmUserEmailAsync(
        int id, 
        string token, 
        CancellationToken cancellationToken = default);
}