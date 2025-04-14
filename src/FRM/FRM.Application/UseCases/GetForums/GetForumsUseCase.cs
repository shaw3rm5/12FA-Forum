using Forum.Application.Dtos;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IGetForumStorage _storage;

    public GetForumsUseCase(IGetForumStorage storage)
    {
        _storage = storage;
    }

    public async Task<IEnumerable<ForumDto>> Execute(CancellationToken ct) => await _storage.GetForums(ct);
}