using Domain.Models;

namespace Forum.Application.UseCases.GetTopics;

public interface IGetTopicsUseCase
{
    public Task<(IEnumerable<Topic> resources, int totalCount)> Execute(
        GetTopicsCommand command, CancellationToken cancellationToken);
}