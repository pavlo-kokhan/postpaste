namespace Shared.Result.Results;

public enum ResultStatus
{
    Ok,
    ValidationError,
    BadRequest,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    InternalError,
}