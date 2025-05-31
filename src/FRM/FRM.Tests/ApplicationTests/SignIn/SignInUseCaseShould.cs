using Forum.Application.Authentication;
using Forum.Application.Exceptions;
using Forum.Application.UseCases.SignIn;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using FluentValidation;
using FluentValidation.Results;
using FluentAssertions;
using System.Text;

public class SignInUseCaseTests
{
    private readonly Mock<ISignInStorage> _signInStorage = new();
    private readonly Mock<IValidator<SignInCommand>> _validator = new();
    private readonly Mock<IPasswordManager> _passwordManager = new();
    private readonly Mock<ISymmetricEncryptor> _encryptor = new();
    private readonly AuthenticationConfiguration _config = new() { Base64Key = "PdiAw9FvfRGHpWT3iaNjE4GTWY+z4+UDlzLUSh3Yceg="};

    private SignInUseCase CreateUseCase() =>
        new(_signInStorage.Object, _validator.Object, _passwordManager.Object, _encryptor.Object, Options.Create(_config));


    public SignInUseCaseTests()
    {
        
    }
    
    
    [Fact]
    public async Task Execute_ShouldReturnToken_WhenValidCredentials()
    {
        // Arrange
        var command = new SignInCommand("user", "password");
        var recognisedUser = new RecognisedUser
        {
            UserId = Guid.NewGuid(),
            Salt = new byte[] { 1, 2, 3 },
            PasswordHash = new byte[] { 9, 9, 9 }
        };

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult());

        _signInStorage.Setup(s => s.FindUser(command.Login, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(recognisedUser);

        _passwordManager.Setup(p => p.ComparePasswords(command.Password, recognisedUser.Salt, recognisedUser.PasswordHash))
                        .Returns(true);

        _encryptor.Setup(e => e.Encrypt(recognisedUser.UserId.ToString(), _config.Key, It.IsAny<CancellationToken>()))
                  .ReturnsAsync("encryptedToken");

        var useCase = CreateUseCase();

        // Act
        var (identity, token) = await useCase.Execute(command, default);

        // Assert
        identity.UserId.Should().Be(recognisedUser.UserId);
        token.Should().Be("encryptedToken");
    }

    [Fact]
    public async Task Execute_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var command = new SignInCommand("", "");

        var failures = new List<ValidationFailure>
        {
            new("Login", "Login is required") { ErrorCode = ValidationErrors.Empty },
            new("Password", "Password is required") { ErrorCode = ValidationErrors.Empty }
        };

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult(failures));

        var useCase = CreateUseCase();

        // Act
        Func<Task> act = async () => await useCase.Execute(command, default);

        // Assert
        var ex = await act.Should().ThrowAsync<UserNotFoundException>(); }

    [Fact]
    public async Task Execute_ShouldThrowUserNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var command = new SignInCommand("nonexistent", "password");

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult());

        _signInStorage.Setup(s => s.FindUser(command.Login, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((RecognisedUser?)null);

        var useCase = CreateUseCase();

        // Act
        Func<Task> act = async () => await useCase.Execute(command, default);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"*{command.Login}*");
    }

    [Fact]
    public async Task Execute_ShouldThrowIncorrectPasswordException_WhenPasswordIsInvalid()
    {
        // Arrange
        var command = new SignInCommand("user", "wrongpassword");
        var recognisedUser = new RecognisedUser
        {
            UserId = Guid.NewGuid(),
            Salt = new byte[] { 1, 2, 3 },
            PasswordHash = new byte[] { 9, 9, 9 }
        };

        _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new ValidationResult());

        _signInStorage.Setup(s => s.FindUser(command.Login, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(recognisedUser);

        _passwordManager.Setup(p => p.ComparePasswords(command.Password, recognisedUser.Salt, recognisedUser.PasswordHash))
                        .Returns(false);

        var useCase = CreateUseCase();

        // Act
        Func<Task> act = async () => await useCase.Execute(command, default);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>()
            .WithMessage("*wrongpassword*");
    }
    
    
    
}
