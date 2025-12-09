using FluentValidation;
using MediatR;
using Shared.Result.Results;

namespace Post.Api.Pipelines;

public class ValidationPipeline<TRequest> : IPipelineBehavior<TRequest, Result> 
    where TRequest : IRequest<Result>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationPipeline(IServiceProvider serviceProvider) 
        => _serviceProvider = serviceProvider;

    public async Task<Result> Handle(
        TRequest request, 
        RequestHandlerDelegate<Result> next, 
        CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator is null)
            return await next(cancellationToken);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return Result.Failure(
                ResultStatus.ValidationError,
                validationResult.Errors
                    .Select(e => Error.CreatePropertyValidationError(
                        e.ErrorCode,
                        e.PropertyName,
                        e.ErrorMessage))
                    .ToList());
        
        return await next(cancellationToken);
    }
}