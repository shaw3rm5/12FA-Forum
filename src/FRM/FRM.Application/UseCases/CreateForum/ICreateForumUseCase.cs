using Forum.Application.Dtos;
using Forum.Application.UseCases.CreateTopic;

namespace Forum.Application.UseCases.CreateForum;

public interface ICreateForumUseCase
{
    public Task<ForumDto> Execute(ForumCreateCommand command, CancellationToken cancellationToken);
}