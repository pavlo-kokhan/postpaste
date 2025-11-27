using Microsoft.AspNetCore.Mvc;

namespace Post.Api.Extensions;

public static class ExceptionToProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this Exception exception, bool includeExceptionDetails = false) 
        => new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Internal server error.",
            Type = "https://httpstatuses.io/500",
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = new Dictionary<string, string?[]>
                {
                    { "Exception messages", [includeExceptionDetails ? exception.Message : null] }
                }
            }
        };
}