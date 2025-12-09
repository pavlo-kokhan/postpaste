using MediatR;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.User.ResendConfirmationEmail;

public record ResendUserConfirmationEmailCommand(string Email, string Password) : IRequest<Result>
{
    public class Handler : IRequestHandler<ResendUserConfirmationEmailCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService) 
            => _identityService = identityService;

        public async Task<Result> Handle(ResendUserConfirmationEmailCommand request, CancellationToken cancellationToken) 
            => await _identityService.ResendConfirmationEmailAsync(request.Email, request.Password, cancellationToken);
    }
}