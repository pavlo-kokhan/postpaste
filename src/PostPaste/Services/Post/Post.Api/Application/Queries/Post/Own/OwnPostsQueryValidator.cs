using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Queries.Post.Own;

public class OwnPostsQueryValidator : AbstractValidator<OwnPostsQuery>
{
    public OwnPostsQueryValidator()
    {
        RuleFor(x => x.Page)
            .Page();
        
        RuleFor(x => x.PageSize)
            .PageSize();
        
        RuleFor(x => x.FolderId)
            .Id(x => x.FolderId);
    }
}