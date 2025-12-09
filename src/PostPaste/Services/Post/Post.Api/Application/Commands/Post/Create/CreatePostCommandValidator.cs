using FluentValidation;
using Post.Domain.Entities.Post;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.Post.Create;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator(IValidator<PostCategoryValueObject> postCategoryValidator)
    {
        RuleFor(x => x.Name)
            .PostName();

        RuleFor(x => x.Category)
            .NotEmpty()
            .SetValidator(postCategoryValidator);
        
        RuleFor(x => x.Tags)
            .Tags();
        
        RuleFor(x => x.Password)
            .PostPassword(x => x.Password);
        
        RuleFor(x => x.ExpirationDate)
            .OptionalExpirationDate(x => x.ExpirationDate);
        
        RuleFor(x => x.FolderId)
            .Id(x => x.FolderId);
    }
}