using Forum.Application.Dtos;

namespace Forum.Application.UseCases.GetTopics;

public interface IGetTopicsUseCase
{
    public Task<(IEnumerable<TopicDto> resources, int totalCount)> Execute(
        GetTopicsCommand command, CancellationToken cancellationToken);
}