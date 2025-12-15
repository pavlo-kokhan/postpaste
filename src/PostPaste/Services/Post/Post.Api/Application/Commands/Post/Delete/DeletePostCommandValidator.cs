using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.Post.Delete;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.PostId)
            .RequiredId();
    }
}