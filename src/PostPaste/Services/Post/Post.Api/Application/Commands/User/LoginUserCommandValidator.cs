using FluentValidation;
using Post.Domain;

namespace Post.Api.Application.Commands.User;

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