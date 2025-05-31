using FluentValidation;
using Forum.Application.Authentication;
using Forum.Application.UseCases.SignIn;

namespace Forum.Application.UseCases.SignUp;

public class SignUpUseCase : ISignUpUseCase
{
    private readonly IValidator<SignUpCommand> _validator;
    private readonly ISignUpStorage _storage;
    private readonly IPasswordManager _passwordManager;

    public SignUpUseCase(
        IValidator<SignUpCommand> validator,
        ISignUpStorage storage,
        IPasswordManager passwordManager)
    {
        _validator = validator;
        _storage = storage;
        _passwordManager = passwordManager;
    }
    
    public async Task<IIdentity> Execute(SignUpCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var (salt, hash) = _passwordManager.GeneratePasswordParts(command.Password);
        
        var userId = await _storage.AddUserAsync(command.Login, salt, hash, cancellationToken);
        
        return new UserIdentity(userId, Guid.Empty);
    }
}