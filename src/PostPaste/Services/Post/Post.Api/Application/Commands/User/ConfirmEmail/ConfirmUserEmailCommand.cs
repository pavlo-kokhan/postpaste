using MediatR;
using Post.Api.Application.Responses.User;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Commands.User.ConfirmEmail;

public record ConfirmUserEmailCommand(int UserId, string Token) : IRequest<Result<EmailConfirmationResponseDto>>
{
    public class Handler : IRequestHandler<ConfirmUserEmailCommand, Result<EmailConfirmationResponseDto>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService) 
            => _identityService = identityService;

        public async Task<Result<EmailConfirmationResponseDto>> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var confirmationResult = await _identityService.ConfirmUserEmailAsync(request.UserId, request.Token, cancellationToken);

            if (confirmationResult.IsFailure)
                return Result<EmailConfirmationResponseDto>.Failure(
                    confirmationResult.Status, 
                    confirmationResult.Errors);

            return new EmailConfirmationResponseDto(confirmationResult.Data);
        }
    }
}