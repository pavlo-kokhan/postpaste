using Microsoft.AspNetCore.Authentication.BearerToken;
using Post.Domain.Entities.User;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Services.Abstract;

public interface IJwtTokenService
{
    public Task<Result<AccessTokenResponse>> GenerateAccessTokenAsync(UserEntity userEntity, CancellationToken cancellationToken = default);
}