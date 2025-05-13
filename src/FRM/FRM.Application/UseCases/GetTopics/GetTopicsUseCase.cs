using FluentValidation;
using Forum.Application.Dtos;

namespace Forum.Application.UseCases.GetTopics;

public class GetTopicsUseCase : IGetTopicsUseCase
{
    private readonly IValidator<GetTopicsCommand> _validator;
    private readonly IGetTopicsStorage _storage;

    public GetTopicsUseCase(
        IValidator<GetTopicsCommand> validator,
        IGetTopicsStorage storage)
    {
        _validator = validator;
        _storage = storage;
    }
    
    public async Task<(IEnumerable<TopicDto> resources, int totalCount)> Execute(GetTopicsCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var topics = await _storage.GetTopicsAsync(command.ForumId, command.Skip, command.Take, cancellationToken);
        
        return (topics, topics.Count());

    }
}