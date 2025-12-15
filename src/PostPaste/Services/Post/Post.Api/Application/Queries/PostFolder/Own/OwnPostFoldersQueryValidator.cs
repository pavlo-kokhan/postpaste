using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Queries.PostFolder.Own;

public class OwnPostFoldersQueryValidator : AbstractValidator<OwnPostFoldersQuery>
{
    public OwnPostFoldersQueryValidator()
    {
        RuleFor(x => x.Page)
            .Page();
        
        RuleFor(x => x.PageSize)
            .PageSize();
    }
}