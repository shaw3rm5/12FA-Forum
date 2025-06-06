using Forum.Application.Dtos;
using Forum.Application.Monitoring;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.UseCases.GetForums;

public class GetForumsUseCase : IGetForumsUseCase
{
    private readonly IGetForumStorage _storage;
    private readonly DomainMetrics _domainMetrics;

    public GetForumsUseCase(
        IGetForumStorage storage,
        DomainMetrics domainMetrics)
    {
        _storage = storage;
        _domainMetrics = domainMetrics;
    }

    public async Task<IEnumerable<ForumDto>> Execute(CancellationToken ct)
    {
        try
        {
            var result = await _storage.GetForums(ct);
            _domainMetrics.ForumFetched(true);
            return result;
        }
        catch
        {
            _domainMetrics.ForumFetched(false);
            throw;
        }
    }
}