using Forum.Domain.Dtos;
using Forum.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Forum.Domain.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly ForumDbContext _dbContext;

    public GetForumsUseCase(ForumDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<ForumDto>> Execute(CancellationToken ct)
    {
        return await _dbContext.Forums
            .Select(f => new ForumDto
            {
                Id = f.Id,
                Title = f.Title,
            })
            .ToArrayAsync(ct);
    }
}