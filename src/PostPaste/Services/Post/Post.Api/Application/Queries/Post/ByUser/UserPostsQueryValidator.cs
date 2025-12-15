using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Queries.Post.ByUser;

public class UserPostsQueryValidator : AbstractValidator<UserPostsQuery>
{
    public UserPostsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredId();
        
        RuleFor(x => x.FolderId)
            .Id(x => x.FolderId);
        
        RuleFor(x => x.Page)
            .Page();
        
        RuleFor(x => x.PageSize)
            .PageSize();
    }
}