using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.User.ChangePassword;

public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
{
    public ChangeUserPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .UserEmail();
        
        RuleFor(x => x.OldPassword)
            .UserPassword();
        
        RuleFor(x => x.NewPassword)
            .UserPassword();
    }
}