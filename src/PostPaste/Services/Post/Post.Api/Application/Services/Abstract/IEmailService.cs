using Shared.Result.Results;

namespace Post.Api.Application.Services.Abstract;

public interface IEmailService
{
    Task<Result> SendUserRegistrationConfirmationEmailAsync(string email, string confirmationUrl, CancellationToken cancellationToken = default);
}