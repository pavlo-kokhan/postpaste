using Shared.Result.Results;

namespace Post.Api.Application.Constants.Errors;

public static class PasswordErrors
{
    public static Error PasswordEmpty => Error.Create("PASSWORD_IS_EMPTY");
}