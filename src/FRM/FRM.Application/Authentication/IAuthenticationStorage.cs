namespace Forum.Application.Authentication;

public interface IAuthenticationStorage
{
    public Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken);
}