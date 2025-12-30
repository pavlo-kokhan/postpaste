using Microsoft.AspNetCore.Mvc;
using Shared.Result.Results;
using Shared.Result.Results.Generic.Abstract;

namespace Post.Api.Extensions;

public static class ResultToMvcResultExtensions
{
    public static IActionResult ToMvcResult(this Result result)
        => CreateResult(result);

    private static IActionResult CreateResult(Result result)
    {
        if (result.IsSuccess)
        {
            if (result is IResult<object> objectResult)
                return new OkObjectResult(objectResult.Data);

            return new OkResult();
        }

        return CreateProblemResult(result);
    }
    
    private static IActionResult CreateProblemResult(Result result)
    {
        var (statusCode, title, problemUri) = GetFailureMetadata(result.Status);
        
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = problemUri,
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = result.Errors
            }
        };
        
        return result.Status switch
        {
            ResultStatus.BadRequest => new BadRequestObjectResult(problemDetails),
            ResultStatus.ValidationError => new BadRequestObjectResult(problemDetails),
            ResultStatus.Unauthorized => new UnauthorizedObjectResult(problemDetails),
            ResultStatus.Forbidden => new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status403Forbidden
            },
            ResultStatus.NotFound => new NotFoundObjectResult(problemDetails),
            ResultStatus.Conflict => new ConflictObjectResult(problemDetails),
            ResultStatus.InternalError => new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
            _ => throw new InvalidOperationException("Failure cannot contain other success status.")
        };
    }

    private static (int StatusCode, string Title, string problemUri) GetFailureMetadata(ResultStatus status) 
        => status switch
        {
            ResultStatus.BadRequest => (StatusCodes.Status400BadRequest, "Bad Request", "https://httpstatuses.io/400"),
            ResultStatus.ValidationError => (StatusCodes.Status400BadRequest, "Validation Error", "https://httpstatuses.io/400"),
            ResultStatus.Unauthorized => (StatusCodes.Status401Unauthorized, "Unauthorized", "https://httpstatuses.io/401"),
            ResultStatus.Forbidden => (StatusCodes.Status403Forbidden, "Forbidden", "https://httpstatuses.io/403"),
            ResultStatus.NotFound => (StatusCodes.Status404NotFound, "Not Found", "https://httpstatuses.io/404"),
            ResultStatus.Conflict => (StatusCodes.Status409Conflict, "Conflict", "https://httpstatuses.io/409"),
            ResultStatus.InternalError => (StatusCodes.Status500InternalServerError, "Internal server error", "https://httpstatuses.io/500"),
            _ => throw new InvalidOperationException("Failure cannot contain other success status.")
        };
}