using Domain.Models;
using Forum.Application.Dtos;
using Forum.Application.UseCases.GetForums;
using Forum.Application.UseCases.GetTopics;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Forum.Application.Storage;

public class GetTopicsStorage : IGetTopicsStorage
{
    private readonly IRepository<Topic> _repository;
    private readonly IGetForumStorage _forumStorage;
    private readonly IMemoryCache _memoryCache;

    public GetTopicsStorage(
        IRepository<Topic> repository,
        IGetForumStorage forumStorage,
        IMemoryCache  memoryCache)
    {
        _repository = repository;
        _forumStorage = forumStorage;
        _memoryCache = memoryCache;
    }


    public async Task<IEnumerable<TopicDto>> GetTopicsAsync(Guid forumId, int skip, int take, CancellationToken cancellationToken)
    {
        
        var cacheKey = $"{nameof(GetTopicsAsync)}_{forumId}_{skip}_{take}";
        
        await _forumStorage.ThrowIfForumNotFound(forumId, cancellationToken);

        return (await _memoryCache.GetOrCreateAsync<IEnumerable<TopicDto>>(
            key: cacheKey, 
            factory: async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
            return await _repository
                .Where(t => t.ForumId == forumId)
                .AsNoTracking()
                .Select(t => new TopicDto()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Author = t.Author.UserName,
                    CreatedAt = t.CreatedAt,
                    ForumId = t.ForumId
                })
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken);
        })
            )!;

    }
}