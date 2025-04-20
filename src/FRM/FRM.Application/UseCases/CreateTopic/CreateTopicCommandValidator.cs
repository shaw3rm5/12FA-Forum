using FluentValidation;

namespace Forum.Application.UseCases.CreateTopic;

public class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId)
            .NotEmpty().WithMessage("Forum Id cannot be empty");
        RuleFor(c => c.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(200).WithMessage("Title length cannot be less than 200 characters");
    }
}