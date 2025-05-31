using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.Exceptions;
using Microsoft.Extensions.Options;

namespace Forum.Application.UseCases.SignIn;

public class SignInUseCase : ISignInUseCase
{
    private readonly IValidator<SignInCommand> _validator;
    private readonly IPasswordManager _passwordManager;
    private readonly ISymmetricEncryptor _encryptor;
    private readonly ISignInStorage _signInStorage;
    private readonly AuthenticationConfiguration _configuration;

    public SignInUseCase(
        ISignInStorage signInStorage,
        IValidator<SignInCommand> validator,
        IPasswordManager passwordManager,
        ISymmetricEncryptor encryptor,
        IOptions<AuthenticationConfiguration> configurationOptions)
    {
        _validator = validator;
        _passwordManager = passwordManager;
        _encryptor = encryptor;
        _signInStorage = signInStorage;
        _configuration = configurationOptions.Value;
    }

    public async Task<(IIdentity identity, string authToken)> Execute(
        SignInCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var recognisedUser = await GetUser(command, cancellationToken);
        
        await VerifyPassword(recognisedUser, command.Password, cancellationToken);

        // TODO: expiration moment is ugly
        var sessionId = await _signInStorage.CreateSession(recognisedUser.UserId, 
            DateTimeOffset.UtcNow + TimeSpan.FromHours(1),
            cancellationToken);
        
        var authToken = await _encryptor.Encrypt(
            sessionId.ToString(), _configuration.Key, cancellationToken);
        var userIdentity = new UserIdentity(recognisedUser.UserId, sessionId);

        return (userIdentity, authToken);

    }

    private async Task<RecognisedUser> GetUser(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await _signInStorage.FindUser(command.Login, cancellationToken);
        if (user is null)
            throw new UserNotFoundException(command.Login);
        return user;
    }

    private Task<bool> VerifyPassword(RecognisedUser user, string password, CancellationToken cancellationToken)
    {
        var passwordMatches = _passwordManager.ComparePasswords(
            password, user.Salt, user.PasswordHash);
        if (!passwordMatches)
            throw new IncorrectPasswordException(password);
        return Task.FromResult(passwordMatches);
    }
}