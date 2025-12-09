using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.User.ResendConfirmationEmail;

public class ResendUserConfirmationEmailCommandValidator : AbstractValidator<ResendUserConfirmationEmailCommand>
{
    public ResendUserConfirmationEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .UserEmail();
        
        RuleFor(x => x.Password)
            .UserPassword();
    }
}