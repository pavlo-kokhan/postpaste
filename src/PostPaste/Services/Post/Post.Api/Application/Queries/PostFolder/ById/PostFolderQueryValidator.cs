using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Queries.PostFolder.ById;

public class PostFolderQueryValidator : AbstractValidator<PostFolderQuery>
{
    public PostFolderQueryValidator()
    {
        RuleFor(p => p.Id)
            .RequiredId();
    }
}