using Forum.Domain.Dtos;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Domain.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IRepository<Infrastructure.Forum> _repository;

    public GetForumsUseCase(IRepository<Infrastructure.Forum> repository )
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<ForumDto>> Execute(CancellationToken ct)
    {
        return await _repository
            .Select(f => new ForumDto
            {
                Id = f.Id,
                Title = f.Title,
            })
            .ToArrayAsync(ct);
    }
}