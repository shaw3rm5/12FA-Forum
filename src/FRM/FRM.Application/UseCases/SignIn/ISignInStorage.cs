using Domain.Models;
using Forum.Application.Authentication;

namespace Forum.Application.UseCases.SignIn;

public interface ISignInStorage
{
    public Task<RecognisedUser?> FindUser(string login,  CancellationToken cancellationToken);
    
    public Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken);
    
}