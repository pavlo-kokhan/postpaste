using FluentValidation;
using Post.Domain;

namespace Post.Api.Application.Commands.User;

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