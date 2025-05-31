using Domain.Models;
using Forum.Application.Authentication;
using Forum.Application.UseCases.SignIn;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Storage;

public class SignInStorage : ISignInStorage
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Session> _sessionRepository;
    private readonly IGuidFactory _guidFactory;

    public SignInStorage(
        IRepository<User> userRepository,
        IRepository<Session> sessionRepository,
        IGuidFactory guidFactory)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _guidFactory = guidFactory;
    }
    public async Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken)
    {
        return await _userRepository
            .Where(u => u.UserName == login)
            .AsNoTracking()
            .Select(u =>  new RecognisedUser
            {
                UserId = u.Id,
                PasswordHash = u.PasswordHash,
                Salt = u.Salt,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> CreateSession(Guid userId, DateTimeOffset expirationMoment, CancellationToken cancellationToken)
    {
        var session = new Session
        {
            Id = _guidFactory.CreateGuid(),
            ExpiredAt = expirationMoment,
            UserId = userId
        };
        await _sessionRepository.AddAsync(session, cancellationToken);

        return session.Id;
    }
}