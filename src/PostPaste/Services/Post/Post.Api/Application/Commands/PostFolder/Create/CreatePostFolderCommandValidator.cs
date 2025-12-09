using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.PostFolder.Create;

public class CreatePostFolderCommandValidator : AbstractValidator<CreatePostFolderCommand>
{
    public CreatePostFolderCommandValidator()
    {
        RuleFor(x => x.Name)
            .PostFolderName();
    }
}