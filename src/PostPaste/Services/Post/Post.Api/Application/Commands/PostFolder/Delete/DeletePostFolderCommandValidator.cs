using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.PostFolder.Delete;

public class DeletePostFolderCommandValidator : AbstractValidator<DeletePostFolderCommand>
{
    public DeletePostFolderCommandValidator()
    {
        RuleFor(x => x.Id)
            .RequiredId();
    }
}