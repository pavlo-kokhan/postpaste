using FluentValidation;

namespace Post.Domain.Entities.Post;

public class PostEntityValidator : AbstractValidator<PostEntity>
{
    public PostEntityValidator(IValidator<PostCategoryValueObject> categoryValidator)
    {
        // todo: add validation rules
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .SetValidator(categoryValidator);
        
        RuleFor(x => x.ExpirationDate)
            .Must(x => x is not null || x >= DateTime.UtcNow);
    }
}