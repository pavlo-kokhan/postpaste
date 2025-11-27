using System.Linq.Expressions;
using FluentValidation;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Domain;

public static class ValidationExtensions
{
    public static Result<TEntity> ToResult<TEntity>(
        this IValidator<TEntity> entityValidator,
        TEntity entity,
        params Expression<Func<TEntity, object?>>[] propertiesSelector)
    {
        var validationResult = propertiesSelector.Length > 0
            ? entityValidator.Validate(
                entity, 
                options => 
                    options.IncludeProperties(propertiesSelector))
            : entityValidator.Validate(entity);

        if (!validationResult.IsValid)
        {
            return Result<TEntity>.Failure(
                ResultStatus.ValidationError,
                validationResult.Errors
                    .DistinctBy(e => e.ErrorCode)
                    .ToDictionary(
                        e => e.ErrorCode,
                        e => Error.CreatePropertyValidationError(
                            e.ErrorCode,
                            e.PropertyName,
                            e.ErrorMessage)));
        }

        return Result<TEntity>.Success(entity);
    }
    
    public static void Id<T>(this IRuleBuilderInitial<T, int?> builderOptions, Func<T, int?> propSelector)
        => builderOptions.NotEmpty().When(t => propSelector(t) is not null);

    public static void RequiredId<T>(this IRuleBuilderInitial<T, int> builderOptions)
        => builderOptions.NotEmpty();
    
    public static void UserEmail<T>(this IRuleBuilder<T, string> builder)
        => builder
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    
    public static void UserPassword<T>(this IRuleBuilder<T, string> builder)
        => builder
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]")
            .Matches("[a-z]")
            .Matches("[0-9]")
            .Must(p => !string.IsNullOrEmpty(p) && p.Distinct().Count() >= 4);
}