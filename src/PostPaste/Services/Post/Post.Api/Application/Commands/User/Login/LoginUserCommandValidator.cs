using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.User.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .UserEmail();
        
        RuleFor(x => x.Password)
            .UserPassword();
    }
}