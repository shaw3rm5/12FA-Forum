using Forum.Application.Dtos;
using Forum.Application.UseCases.GetForums;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Storage;

public class GetForumStorage : IGetForumStorage
{
    private readonly IRepository<Domain.Models.Forum> _forumRepository;

    public GetForumStorage(IRepository<Domain.Models.Forum> forumRepository)
    {
        _forumRepository = forumRepository;
    }
    
    public async Task<IEnumerable<ForumDto>> GetForums(CancellationToken cancellationToken)
    {
        return await _forumRepository
            .AsQueryable()
            .Select(f => new ForumDto
            {
                Id = f.Id,
                Title = f.Title,
            })
            .ToArrayAsync(cancellationToken);
    }
}