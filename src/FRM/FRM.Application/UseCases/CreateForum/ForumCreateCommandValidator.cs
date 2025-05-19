using FluentValidation;
using Forum.Application.Exceptions;

namespace Forum.Application.UseCases.CreateForum;

public class ForumCreateCommandValidator : AbstractValidator<ForumCreateCommand>
{
    public ForumCreateCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithErrorCode(ValidationErrors.Empty)
            .MaximumLength(15).WithErrorCode(ValidationErrors.TooLong);
    }
}