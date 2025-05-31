using System.Security.Cryptography;
using Domain.Models;
using Forum.Application.Exceptions;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Forum.Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ISymmetricDecryptor _decryptor;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IAuthenticationStorage _authenticationStorage;
    private readonly IRepository<User> _userRepository;
    private readonly AuthenticationConfiguration _configurationOptions;
    
    public AuthenticationService(
        ISymmetricDecryptor decryptor,
        IOptions<AuthenticationConfiguration> configurationOptions,
        ILogger<AuthenticationService> logger,
        IAuthenticationStorage authenticationStorage
        )
    {
        _decryptor = decryptor;
        _logger = logger;
        _authenticationStorage = authenticationStorage;
        _configurationOptions = configurationOptions.Value;
    }

    public async Task<IIdentity> AuthenticateAsync(string authToken, CancellationToken cancellationToken)
    {
        
        string sessionIdString;

        try
        {
            sessionIdString = await _decryptor.Decrypt(authToken, _configurationOptions.Key, cancellationToken);
        }
        catch (CryptographicException cryptographicException)
        {
            _logger.LogWarning(cryptographicException, 
                $"can't decrypt auth token {authToken}, maybe someone is trying to forge it");
            return UserIdentity.Guest;
        }

        if (!Guid.TryParse(sessionIdString, out var sessionId))
        {
            return UserIdentity.Guest;
        }

        var session = await _authenticationStorage.FindSession(sessionId, cancellationToken);
        
        if(session is null || session.ExpiredAt < DateTimeOffset.UtcNow)
            return UserIdentity.Guest;
        
        return new UserIdentity(session.UserId, sessionId);

    }
}