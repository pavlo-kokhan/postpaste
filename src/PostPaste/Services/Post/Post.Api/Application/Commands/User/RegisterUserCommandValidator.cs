using FluentValidation;
using Post.Domain;

namespace Post.Api.Application.Commands.User;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .UserEmail();
        
        RuleFor(x => x.Password)
            .UserPassword();
    }
}