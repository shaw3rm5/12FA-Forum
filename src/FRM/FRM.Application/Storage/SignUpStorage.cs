using Domain.Models;
using Forum.Application.Exceptions;
using Forum.Application.UseCases.SignUp;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forum.Application.Storage;

public class SignUpStorage : ISignUpStorage
{
    private readonly IRepository<User> _repository;
    private readonly IGuidFactory _guidFactory;

    public SignUpStorage(
        IRepository<User> repository,
        IGuidFactory guidFactory)
    {
        _repository = repository;
        _guidFactory = guidFactory;
    }
    public async Task<Guid> AddUserAsync(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken)
    {
        await ThrowIfUserAlreadyExists(login, cancellationToken);

        var user = new User
        {
            Id = _guidFactory.CreateGuid(),
            UserName = login,
            Salt = salt,
            PasswordHash = hash
        };

        await _repository
            .AddAsync(user, cancellationToken);

        return user.Id;
    }

    private async Task ThrowIfUserAlreadyExists(string login, CancellationToken cancellationToken)
    {
        var user = await _repository
            .Where(x => x.UserName == login)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
            throw new UserAlreadyExistsException(login);
        
    }
}