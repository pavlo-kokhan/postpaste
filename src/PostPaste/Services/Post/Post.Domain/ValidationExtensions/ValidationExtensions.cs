using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FluentValidation;
using Shared.Result.Results;
using Shared.Result.Results.Generic;

namespace Post.Domain.ValidationExtensions;

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
                    .Select(e => Error.CreatePropertyValidationError(
                        e.ErrorCode,
                        e.PropertyName,
                        e.ErrorMessage))
                    .ToList());
        }

        return Result<TEntity>.Success(entity);
    }
    
    public static void Id<T>(this IRuleBuilderInitial<T, int?> builderOptions, Func<T, int?> propSelector)
        => builderOptions.NotEmpty().When(t => propSelector(t) is not null);

    public static void RequiredId<T>(this IRuleBuilderInitial<T, int> builderOptions)
        => builderOptions.NotEmpty();
    
    public static void UserEmail<T>(this IRuleBuilderInitial<T, string> builder)
        => builder
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    
    public static void UserPassword<T>(this IRuleBuilderInitial<T, string> builder)
        => builder
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]")
            .Matches("[a-z]")
            .Matches("[0-9]")
            .Must(p => !string.IsNullOrEmpty(p) && p.Distinct().Count() >= 4);
    
    public static void PostName<T>(this IRuleBuilderInitial<T, string> builder)
        => builder
            .NotEmpty()
            .MaximumLength(64)
            .Must(name => PostNameRegex.IsMatch(name));
    
    public static void PostFolderName<T>(this IRuleBuilderInitial<T, string> builder)
        => builder
            .NotEmpty()
            .MaximumLength(64)
            .Must(name => PostFolderNameRegex.IsMatch(name));
    
    public static void PostCategoryName<T>(this IRuleBuilderInitial<T, string> builder)
        => builder
            .NotEmpty()
            .MaximumLength(64)
            .Must(name => PostCategoryNameRegex.IsMatch(name));
    
    public static void Tags<T>(this IRuleBuilderInitial<T, IReadOnlyCollection<string>> builder)
        => builder
            .Must(tags => tags.Count <= 10)
            .ForEach(tag => tag.Tag());
    
    private static void Tag<T>(this IRuleBuilder<T, string> builder)
        => builder
            .NotEmpty()
            .MaximumLength(64)
            .Must(tag => !string.IsNullOrWhiteSpace(tag.Trim()))
            .Must(tag => TagRegex.IsMatch(tag));
    
    public static void OptionalExpirationDate<T>(this IRuleBuilder<T, DateTime?> builder, Func<T, DateTime?> propertySelector)
        => builder
            .NotEmpty()
            .Must(date => date >= DateTime.UtcNow)
            .When(date => propertySelector(date) is not null);
    
    public static void PostPassword<T>(this IRuleBuilder<T, string?> builder, Func<T, string?> propertySelector)
        => builder
            .NotEmpty()
            .MinimumLength(4)
            .MaximumLength(64)
            .Must(p => !string.IsNullOrWhiteSpace(p) && p.Distinct().Count() >= 4)
            .When(p => propertySelector(p) is not null);
    
    private static readonly Regex TagRegex = new("^#[a-z0-9_]+$", RegexOptions.Compiled);
    
    private static readonly Regex PostNameRegex = new("^[A-Za-z0-9 ]+$", RegexOptions.Compiled);
    
    private static readonly Regex PostCategoryNameRegex = new("^[A-Za-z0-9 ]+$", RegexOptions.Compiled);
    
    private static readonly Regex PostFolderNameRegex = new("^[A-Za-z0-9 ]+$", RegexOptions.Compiled);
}