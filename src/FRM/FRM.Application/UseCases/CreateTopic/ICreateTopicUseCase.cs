using Domain.Models;
using Forum.Application.Dtos;
using Forum.Infrastructure;

namespace Forum.Application.UseCases.CreateTopic;

public interface ICreateTopicUseCase
{
    public Task<TopicDto> Execute(CreateTopicCommand command, CancellationToken ct);
}