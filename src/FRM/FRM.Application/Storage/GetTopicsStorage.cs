using System.Collections.Immutable;
using Domain.Models;
using Forum.Application.UseCases.GetTopics;
using Forum.Infrastructure.Repository;

namespace Forum.Application.Storage;

public class GetTopicsStorage : IGetTopicsStorage
{
    private readonly IRepository<Topic> _repository;

    public GetTopicsStorage(IRepository<Topic> repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<Topic>> GetTopicsAsync(Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        var xui = _repository.Where(t => t.ForumId == forumId).ToImmutableList();
        return xui;
    }
}