using Forum.Application.Dtos;

namespace Forum.Application.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    public Task<TopicDto> Execute(CreateTopicCommand command, CancellationToken cancellationToken);
}