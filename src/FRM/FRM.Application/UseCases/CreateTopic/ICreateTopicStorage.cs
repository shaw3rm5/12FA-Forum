using Forum.Infrastructure;

namespace Forum.Application.UseCases.CreateTopic;

public interface ICreateTopicStorage
{
    public Task<bool> ForumExists(Guid forumId, CancellationToken cancellationToken);
    public Task<Topic> CreateTopic(Guid forumId, Guid userId, string title, CancellationToken cancellationToken);
}