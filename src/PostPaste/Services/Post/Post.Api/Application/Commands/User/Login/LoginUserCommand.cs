using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Commands.User.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<AccessTokenResponse>>
{
    public class Handler : IRequestHandler<LoginUserCommand, Result<AccessTokenResponse>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService) 
            => _identityService = identityService;

        public async Task<Result<AccessTokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _identityService.LoginUserAsync(request.Email, request.Password, cancellationToken);

            if (loginResult.IsFailure)
                return loginResult;

            return loginResult.Data;
        }
    }
}