using MediatR;
using Post.Api.Application.Responses;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results.Generic;

namespace Post.Api.Application.Commands.User;

public record ConfirmUserEmailCommand(int UserId, string Token) : IRequest<Result<UserEmailConfirmationResponseDto>>
{
    public class Handler : IRequestHandler<ConfirmUserEmailCommand, Result<UserEmailConfirmationResponseDto>>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService) 
            => _identityService = identityService;

        public async Task<Result<UserEmailConfirmationResponseDto>> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
        {
            var confirmationResult = await _identityService.ConfirmUserEmailAsync(request.UserId, request.Token, cancellationToken);

            if (confirmationResult.IsFailure)
                return Result<UserEmailConfirmationResponseDto>.Failure(
                    confirmationResult.Status, 
                    confirmationResult.Errors);

            return new UserEmailConfirmationResponseDto(confirmationResult.Data);
        }
    }
}