using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class IdentityErrors
{
    public static readonly Error UserAlreadyExists = Error.Create("USER_ALREADY_EXISTS");
    
    public static readonly Error UserCreationFailed = Error.Create("USER_CREATION_FAILED");
    
    public static readonly Error RoleAssignmentFailed = Error.Create("ROLE_ASSIGNMENT_FAILED");
    
    public static readonly Error UserNotFound = Error.Create("USER_NOT_FOUND");
    
    public static readonly Error InvalidPassword = Error.Create("INVALID_USER_PASSWORD");
    
    public static readonly Error PasswordChangeFailed = Error.Create("PASSWORD_CHANGE_FAILED");
    
    public static readonly Error UserConfirmationFailed = Error.Create("USER_CONFIRMATION_FAILED");
    
    public static readonly Error EmailNotConfirmed = Error.Create("EMAIL_NOT_CONFIRMED");
    
    public static readonly Error EmailAlreadyConfirmed = Error.Create("EMAIL_ALREADY_CONFIRMED");
}