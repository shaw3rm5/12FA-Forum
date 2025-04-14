using Forum.Application.Dtos;

namespace Forum.Application.UseCases.CreateTopic;

public interface ICreateTopicStorage
{
    public Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken);
    public Task<TopicDto> CreateTopic(Guid forumId, Guid authorId, string title, CancellationToken cancellationToken);
}