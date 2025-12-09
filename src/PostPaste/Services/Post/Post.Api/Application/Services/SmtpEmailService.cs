using FluentEmail.Core;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;

namespace Post.Api.Application.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public SmtpEmailService(IFluentEmail fluentEmail) 
        => _fluentEmail = fluentEmail;

    public async Task<Result> SendUserRegistrationConfirmationEmailAsync(
        string email, 
        string confirmationUrl, 
        CancellationToken cancellationToken = default)
    {
        var body = $"""
                    <html>
                      <body>
                        <h1>You are one step away from confirming your email!</h1>
                        <p>Confirm your email using this url:</p>
                        <p><a href="{confirmationUrl}">{confirmationUrl}</a></p>
                        <p>If you did not register, simply ignore this email.</p>
                      </body>
                    </html>
                    """;
        
        return await SendEmailAsync(
            email, 
            "Email Confirmation from PostPaste", 
            body, 
            true, 
            cancellationToken);
    }
    
    private async Task<Result> SendEmailAsync(
        string email, 
        string subject, 
        string body, 
        bool isHtml = false, 
        CancellationToken cancellationToken = default)
    {
        var response = await _fluentEmail
            .To(email)
            .Subject(subject)
            .Body(body, isHtml)
            .SendAsync(cancellationToken);

        if (!response.Successful)
            return Result.ValidationFailure(EmailErrors.EmailSendingFailed);
        
        return Result.Success();
    }
}