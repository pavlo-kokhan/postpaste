using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Domain.Entities.Post;

public class PostEntityValidator : AbstractValidator<PostEntity>
{
    public PostEntityValidator(IValidator<PostCategoryValueObject> categoryValidator)
    {
        RuleFor(x => x.Name)
            .PostName();
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .SetValidator(categoryValidator);
        
        RuleFor(x => x.Tags)
            .Tags();
        
        RuleFor(x => x.ExpirationDate)
            .OptionalExpirationDate(x => x.ExpirationDate);
        
        RuleFor(x => x.OwnerId)
            .RequiredId();
        
        RuleFor(x => x.FolderId)
            .Id(x => x.FolderId);
    }
}