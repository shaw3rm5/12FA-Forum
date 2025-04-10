using Forum.Domain.Dtos;
using Forum.Infrastructure;

namespace Forum.Domain.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    public Task<TopicDto> Execute(Guid forumId, string title, Guid userId, CancellationToken ct);
}