using Microsoft.AspNetCore.Mvc;
using Shared.Result.Results;

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
                ["errors"] = new[]
                {
                    Error.CreateUnknownError(
                        "INTERNAL_SERVER_ERROR",
                        includeExceptionDetails ? exception.Message : "Unknown error")
                }
            }
        };
}