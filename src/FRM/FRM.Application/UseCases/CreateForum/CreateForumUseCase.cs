using FluentValidation;
using Forum.Application.Dtos;

namespace Forum.Application.UseCases.CreateForum;

public class CreateForumUseCase : ICreateForumUseCase
{
    private readonly IValidator<ForumCreateCommand> _validator;
    private readonly ICreateForumStorage _storage;

    public CreateForumUseCase(
        IValidator<ForumCreateCommand> validator, 
        ICreateForumStorage storage)
    {
        _validator = validator;
        _storage = storage;
    }
    
    public async Task<ForumDto> Execute(ForumCreateCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var forum = await _storage.CreateForumAsync(command.Title, cancellationToken);

        return new ForumDto()
        {
            Id = forum.Id,
            Title = forum.Title,
        };
    }
}