using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class PostErrors
{
    public static readonly Error NotFound = Error.Create("POST_NOT_FOUND");
    
    public static readonly Error PasswordRequired = Error.Create("PASSWORD_REQUIRED");
    
    public static readonly Error InvalidPassword = Error.Create("INVALID_PASSWORD");
    
    public static readonly Error Forbidden = Error.Create("POST_FORBIDDEN");
}