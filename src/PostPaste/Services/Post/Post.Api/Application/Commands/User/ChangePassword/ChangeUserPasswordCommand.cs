using MediatR;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;

namespace Post.Api.Application.Commands.User.ChangePassword;

public record ChangeUserPasswordCommand(string Email, string OldPassword, string NewPassword) : IRequest<Result>
{
    public class Handler : IRequestHandler<ChangeUserPasswordCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService) 
            => _identityService = identityService;

        public async Task<Result> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken) 
            => await _identityService.ChangePasswordAsync(request.Email, request.OldPassword, request.NewPassword, cancellationToken);
    }
}