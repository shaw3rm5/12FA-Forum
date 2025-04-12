using Forum.Application.Dtos;
using Forum.Infrastructure;

namespace Forum.Application.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    public Task<Topic> Execute(Guid forumId, string title, CancellationToken ct);
}