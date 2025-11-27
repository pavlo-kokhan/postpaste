using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Post.Api.Extensions;
using Shared.Result.Results;
using Shared.Result.Results.Generic.Abstract;

namespace Post.Api.Filters;

public class ResultActionFilter : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ActionResultWrapper actionResult)
            return;

        if (actionResult.Result.IsSuccess)
        {
            if (actionResult.Result is IResult<object> objectResult)
            {
                context.Result = new OkObjectResult(objectResult.Data);

                return;
            }
            
            context.Result = new OkResult();
            
            return;
        }

        var problemDetails = actionResult.Result.ToProblemDetails();
        
        context.Result = actionResult.Result.Status switch
        {
            ResultStatus.ValidationError => context.Result = new BadRequestObjectResult(problemDetails),
            ResultStatus.NotFound => context.Result = new NotFoundObjectResult(problemDetails),
            ResultStatus.InternalError => context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
            _ => context.Result = new BadRequestObjectResult(problemDetails)
        };
    }
}