using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Queries.PostFolder.ByUser;

public class UserPostFoldersQueryValidator : AbstractValidator<UserPostFoldersQuery>
{
    public UserPostFoldersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredId();
        
        RuleFor(x => x.Page)
            .Page();
        
        RuleFor(x => x.PageSize)
            .PageSize();
    }
}