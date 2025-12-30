using Microsoft.AspNetCore.Mvc.Filters;
using Post.Api.Extensions;

namespace Post.Api.Filters;

public class ResultActionFilter : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ActionResultWrapper actionResult)
            return;

        context.Result = actionResult.Result.ToMvcResult();
    }
}