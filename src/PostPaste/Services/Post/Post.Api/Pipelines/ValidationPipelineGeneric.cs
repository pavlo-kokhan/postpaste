using FluentValidation;
using MediatR;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Api.Pipelines;

public class ValidationPipelineGeneric<TRequest, TData> : IPipelineBehavior<TRequest, Result<TData>>
    where TRequest : IRequest<Result<TData>>
{
    private readonly IServiceProvider _serviceProvider;
    
    public ValidationPipelineGeneric(IServiceProvider serviceProvider) 
        => _serviceProvider = serviceProvider;
    
    public async Task<Result<TData>> Handle(
        TRequest request, 
        RequestHandlerDelegate<Result<TData>> next, 
        CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator is null)
            return await next(cancellationToken);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return Result<TData>.ValidationFailure(
                validationResult.Errors
                    .Select(e => Error.CreatePropertyValidationError(
                        e.ErrorCode,
                        e.PropertyName,
                        e.ErrorMessage))
                    .ToList());
        
        return await next(cancellationToken);
    }
}