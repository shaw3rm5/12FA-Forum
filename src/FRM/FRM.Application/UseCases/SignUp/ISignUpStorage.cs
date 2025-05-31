using Domain.Models;
using Forum.Application.Authentication;
using Npgsql.Replication;

namespace Forum.Application.UseCases.SignUp;

public interface ISignUpStorage
{
    public Task<Guid> AddUserAsync(string login, byte[] salt, byte[] hash, CancellationToken cancellationToken);
}
