using FluentValidation;
using Forum.Application.Exceptions;

namespace Forum.Application.UseCases.GetTopics;

public class GetTopicsCommandValidator : AbstractValidator<GetTopicsCommand>
{
    public GetTopicsCommandValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode(ValidationErrors.Empty);
        RuleFor(c => c.Skip).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrors.Invalid);
        RuleFor(c => c.Take).GreaterThanOrEqualTo(1).WithErrorCode(ValidationErrors.Invalid);
    }
    
}