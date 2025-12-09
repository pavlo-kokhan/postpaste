using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Domain.Entities.Post;

public class PostCategoryValueObjectValidator : AbstractValidator<PostCategoryValueObject>
{
    public PostCategoryValueObjectValidator()
    {
        RuleFor(x => x.Name)
            .PostCategoryName();
    }
}