namespace Forum.Application.UseCases.CreateForum;
using Forum = Domain.Models.Forum;

public interface ICreateForumStorage
{
    public Task<Forum> CreateForumAsync(string title, CancellationToken cancellationToken);
}