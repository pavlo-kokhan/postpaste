using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.User.ConfirmEmail;

public class ConfirmUserEmailCommandValidator : AbstractValidator<ConfirmUserEmailCommand>
{
    public ConfirmUserEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredId();
        
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}