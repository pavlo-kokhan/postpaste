using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.PostFolder.Update;

public class UpdatePostFolderCommandValidator : AbstractValidator<UpdatePostFolderCommand>
{
    public UpdatePostFolderCommandValidator()
    {
        RuleFor(x => x.Id)
            .RequiredId();
        
        RuleFor(x => x.Name)
            .PostFolderName();
    }
}