using Domain.Models;
using Forum.Application.Dtos;

namespace Forum.Application.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    public Task<IEnumerable<TopicDto>> GetTopicsAsync(Guid forumId, int skip, int take, CancellationToken cancellationToken);
}