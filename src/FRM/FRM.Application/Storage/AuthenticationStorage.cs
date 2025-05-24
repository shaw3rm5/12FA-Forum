using Domain.Models;
using Forum.Application.Authentication;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Storage;

public class AuthenticationStorage : IAuthenticationStorage
{
    private readonly IRepository<User> _userRepository;

    public AuthenticationStorage(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken)
    {
        return _userRepository
            .Where(u => u.Login.Equals(login))
            .AsNoTracking()
            .Select(u => new RecognisedUser
            {
                UserId = u.Id,
                Salt = u.Salt,
                PasswordHash = u.PasswordHash
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}