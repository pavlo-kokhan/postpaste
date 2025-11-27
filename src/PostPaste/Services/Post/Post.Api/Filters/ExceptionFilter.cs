using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Post.Api.Extensions;

namespace Post.Api.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IHostEnvironment _environment;

    public ExceptionFilter(IHostEnvironment environment) 
        => _environment = environment;

    public void OnException(ExceptionContext context)
    {
        var problemDetails = context.Exception.ToProblemDetails(
            _environment.IsDevelopment() 
            || _environment.IsEnvironment("Debug"));

        context.ExceptionHandled = true;
        
        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}