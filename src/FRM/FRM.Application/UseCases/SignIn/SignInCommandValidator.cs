using FluentValidation;
using Forum.Application.Exceptions;

namespace Forum.Application.UseCases.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(r => r.Login).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Empty);
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Empty);
    }
}