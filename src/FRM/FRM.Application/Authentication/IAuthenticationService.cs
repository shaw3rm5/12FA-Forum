namespace Forum.Application.Authentication;

public interface IAuthenticationService
{
    public Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken);
}