namespace Forum.Application.Authentication;

public interface IAuthenticationStorage
{
    public Task<SessionDto?> FindSession(Guid sessionId, CancellationToken cancellationToken);
}