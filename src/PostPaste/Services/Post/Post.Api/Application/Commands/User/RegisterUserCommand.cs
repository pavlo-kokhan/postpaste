using MediatR;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.User;

public record RegisterUserCommand(string Email, string Password) : IRequest<Result>
{
    public class Handler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService) 
            => _identityService = identityService;

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken) 
            => await _identityService.RegisterUserAsync(request.Email, request.Password, cancellationToken);
    }
}