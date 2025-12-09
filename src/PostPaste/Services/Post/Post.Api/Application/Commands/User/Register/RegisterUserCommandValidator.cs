using FluentValidation;
using Post.Domain.ValidationExtensions;

namespace Post.Api.Application.Commands.User.Register;

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