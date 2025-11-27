using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class EmailErrors
{
    public static readonly Error EmailSendingFailed = Error.Create("EMAIL_SENDING_FAILED"); 
}