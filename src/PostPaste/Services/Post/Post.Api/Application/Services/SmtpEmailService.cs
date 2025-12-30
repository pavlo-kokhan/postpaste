using System.Text;
using FluentEmail.Core;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Post.Api.Application.Constants.Errors;
using Post.Api.Application.Options;
using Post.Api.Application.Services.Abstract;
using Shared.Result.Results;

namespace Post.Api.Application.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;
    private readonly EmailUrlsOptions _emailUrlOptions;

    public SmtpEmailService(IFluentEmail fluentEmail, IOptions<EmailUrlsOptions> emailUrlOptions)
    {
        _fluentEmail = fluentEmail;
        _emailUrlOptions = emailUrlOptions.Value;
    }

    public async Task<Result> SendUserConfirmationEmailAsync(
        string email,
        int userId,
        string confirmationToken, 
        CancellationToken cancellationToken = default)
    {
        var encodedConfirmationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationToken));
        
        var queryParameters = new Dictionary<string, string>
        {
            { "id", userId.ToString() },
            { "token", encodedConfirmationToken }
        };
        
        var confirmationUrl = QueryHelpers.AddQueryString(_emailUrlOptions.EmailConfirmationBaseUrl, queryParameters!);
        
        var body = $"""
                    <html>
                      <body>
                        <h1>You are one step away from confirming your email!</h1>
                        <p>Confirm your email using this url:</p>
                        <p><a href="{confirmationUrl}">Click Here to Confirm</a></p>
                        <p>If you did not register, simply ignore this email.</p>
                        <div>
                            <p>For Development:</p>
                            <p>UserId: {userId.ToString()}</p>
                            <p>Token: {encodedConfirmationToken}</p>
                        </div>
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