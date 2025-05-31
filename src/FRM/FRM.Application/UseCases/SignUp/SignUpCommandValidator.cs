using FluentValidation;
using Forum.Application.Exceptions;

namespace Forum.Application.UseCases.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(r => r.Login).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Empty);
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Empty);
    }
}   