using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Queries.Post.ById;

public class PostQueryValidator : AbstractValidator<PostQuery>
{
    public PostQueryValidator()
    {
        RuleFor(p => p.Id)
            .RequiredId();
    }
}