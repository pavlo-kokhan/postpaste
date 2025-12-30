using Shared.Result.Results;

namespace Post.Api.Application.Services.Abstract;

public interface IEmailService
{
    Task<Result> SendUserConfirmationEmailAsync(
        string email,
        int userId,
        string confirmationToken, 
        CancellationToken cancellationToken = default);
}