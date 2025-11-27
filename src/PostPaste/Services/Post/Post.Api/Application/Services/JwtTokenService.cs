using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Post.Api.Application.Options;
using Post.Api.Application.Services.Abstract;
using Post.Domain.Entities.User;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly JwtTokenOptions _jwtTokenOptions;

    public JwtTokenService(UserManager<UserEntity> userManager, IOptions<JwtTokenOptions> jwtTokenOptions)
    {
        _userManager = userManager;
        _jwtTokenOptions = jwtTokenOptions.Value;
    }

    public async Task<Result<AccessTokenResponse>> GenerateAccessTokenAsync(UserEntity userEntity, CancellationToken cancellationToken = default)
    {
        var role = (await _userManager.GetRolesAsync(userEntity)).FirstOrDefault();
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userEntity.Id.ToString())
        };
        
        if (role is not null)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return GetAccessToken(claims);
    }

    private AccessTokenResponse GetAccessToken(List<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims);
        
        var jwtSecurityToken = new JwtSecurityToken(
            claims: claimsIdentity.Claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(_jwtTokenOptions.ExpiresIn),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(_jwtTokenOptions.Key),
                SecurityAlgorithms.HmacSha256));

        return new AccessTokenResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            ExpiresIn = _jwtTokenOptions.ExpiresIn,
            RefreshToken = string.Empty
        };
    }
}