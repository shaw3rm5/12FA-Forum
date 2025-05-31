using Domain.Models;
using Forum.Infrastructure.Repository;

namespace Forum.Application.UseCases.SignOut;

public class SignOutStorage : ISignOutStorage
{
    private readonly IRepository<Session> _sessionsRepository;

    public SignOutStorage(IRepository<Session> sessionsRepository)
    {
        _sessionsRepository = sessionsRepository;
    }
    public async Task RemoveSession(Guid sessionId, CancellationToken cancellationToken)
    {
        await _sessionsRepository
            .DeleteAsync([
                new Session { Id = sessionId }
            ], cancellationToken);
    }
}