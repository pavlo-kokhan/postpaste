using Microsoft.AspNetCore.Mvc;
using Shared.Result.Results;

namespace Post.Api.Extensions;

public static class ResultToProblemDetailsExtensions
{
    public static ProblemDetails ToProblemDetails(this Result result)
        => CreateProblemDetails(result);
    
    private static ProblemDetails CreateProblemDetails(Result result)
    {
        var (statusCode, title) = GetStatusCode(result.Status);
        var problemUri = GetProblemUri(result.Status);
        
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = problemUri,
            Extensions = new Dictionary<string, object?>
            {
                ["errors"] = result.Errors
                    .ToDictionary(
                        e => e.Key, 
                        e => new ErrorResponse(
                            e.Value.Message, 
                            e.Value.PropertyName))
            }
        };
    }

    private static (int StatusCode, string Title) GetStatusCode(ResultStatus status) 
        => status switch
        {
            ResultStatus.ValidationError => (StatusCodes.Status400BadRequest, "Validation Error"),
            ResultStatus.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
            _ => (StatusCodes.Status500InternalServerError, "Internal server Error")
        };
    
    private static string GetProblemUri(ResultStatus status)
        => status switch
        {
            ResultStatus.ValidationError => "https://httpstatuses.io/400",
            ResultStatus.NotFound => "https://httpstatuses.io/404",
            _ => "https://httpstatuses.io/500"
        };
}