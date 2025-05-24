namespace Forum.Application.Authentication;

public interface IAuthenticationService
{
    public Task<(bool isSucces, string authToken)> SignIn(BasicSignInCredentials credentials, CancellationToken cancellationToken);
    
    public Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken);
}