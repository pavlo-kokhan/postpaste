using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Domain.Entities.PostFolder;

public class PostFolderEntityValidator : AbstractValidator<PostFolderEntity>
{
    public PostFolderEntityValidator()
    {
        RuleFor(x => x.Name)
            .PostFolderName();
        
        RuleFor(x => x.OwnerId)
            .RequiredId();
    }
}