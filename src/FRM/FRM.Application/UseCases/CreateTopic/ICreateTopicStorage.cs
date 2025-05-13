using Forum.Application.Dtos;

namespace Forum.Application.UseCases.CreateTopic;

public interface ICreateTopicStorage
{
    public Task<TopicDto> CreateTopic(Guid forumId, Guid authorId, string title, CancellationToken cancellationToken);
}