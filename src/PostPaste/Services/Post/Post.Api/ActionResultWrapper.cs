using Microsoft.AspNetCore.Mvc;
using Shared.Result.Results;

namespace Post.Api;

public class ActionResultWrapper : ActionResult
{
    public ActionResultWrapper(Result result)
    {
        Result = result;
    }
    
    public Result Result { get; }
}