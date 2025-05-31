namespace Forum.Application.UseCases.SignOut;

public interface ISignOutStorage
{
    public Task RemoveSession(Guid sessionId, CancellationToken cancellationToken);
}