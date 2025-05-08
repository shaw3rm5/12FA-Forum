using Domain.Models;

namespace Forum.Application.UseCases.GetTopics;

public interface IGetTopicsStorage
{
    public Task<IEnumerable<Topic>> GetTopicsAsync(Guid forumId, int skip, int take, CancellationToken cancellationToken);
}