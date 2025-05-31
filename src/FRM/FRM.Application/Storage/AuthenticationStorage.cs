using Domain.Models;
using Forum.Application.Authentication;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Storage;

public class AuthenticationStorage : IAuthenticationStorage
{
    private readonly IRepository<Session> _sessionRepository;

    public AuthenticationStorage(IRepository<Session> sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task<SessionDto?> FindSession(Guid sessionId, CancellationToken cancellationToken) =>
        await _sessionRepository
            .Where(s => s.Id == sessionId)
            .Select(s => new SessionDto
            {
                UserId = s.UserId,
                ExpiredAt = s.ExpiredAt
            })
            .FirstOrDefaultAsync(cancellationToken);
}