using Forum.Application.Dtos;
using Forum.Application.Exceptions;
using Forum.Application.UseCases.GetForums;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Forum.Application.Storage;

public class GetForumStorage : IGetForumStorage
{
    private readonly IRepository<Domain.Models.Forum> _forumRepository;
    private readonly IMemoryCache _memoryCache;

    public GetForumStorage(
        IRepository<Domain.Models.Forum> forumRepository,
        IMemoryCache memoryCache)
    {
        _forumRepository = forumRepository;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<ForumDto>> GetForums(CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync<IEnumerable<ForumDto>>(nameof(GetForums), async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120);
            
            return await _forumRepository
                .AsQueryable()
                .Select(f => new ForumDto
                {
                    Id = f.Id,
                    Title = f.Title,
                })
                .ToArrayAsync(cancellationToken);
        }) ?? throw new InvalidOperationException("suchka");

    }
}