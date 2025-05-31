using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Forum.Application.Authentication;
using Forum.Application.Exceptions;
using Forum.Application.UseCases.SignUp;
using Moq;

public class SignUpUseCaseTests
{
    private readonly Mock<IValidator<SignUpCommand>> _validatorMock = new();
    private readonly Mock<ISignUpStorage> _storageMock = new();
    private readonly Mock<IPasswordManager> _passwordManagerMock = new();
    private readonly SignUpUseCase _useCase;

    public SignUpUseCaseTests()
    {
        _useCase = new SignUpUseCase(
            _validatorMock.Object,
            _storageMock.Object,
            _passwordManagerMock.Object
        );
    }
    
    // 

    [Fact]
    public async Task Execute_ShouldReturnUserIdentity_WhenValidInput()
    {
        // Arrange
        var command = new SignUpCommand("user", "password");
        var cancellationToken = CancellationToken.None;
        var salt = new byte[] { 1, 2, 3 };
        var hash = new byte[] { 4, 5, 6 };
        var expectedUserId = Guid.NewGuid();

        _validatorMock
            .Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _passwordManagerMock
            .Setup(p => p.GeneratePasswordParts(command.Password))
            .Returns((salt, hash));

        _storageMock
            .Setup(s => s.AddUserAsync(command.Login, salt, hash, cancellationToken))
            .ReturnsAsync(expectedUserId);

        // Act
        var result = await _useCase.Execute(command, cancellationToken);

        // Assert
        result.UserId.Should().Be(expectedUserId);
    }

    [Fact]
    public async Task Execute_ShouldThrowUserAlreadyExistsException_WhenLoginIsTaken()
    {
        // Arrange
        var command = new SignUpCommand("existingUser", "password");
        var cancellationToken = CancellationToken.None;
        var salt = new byte[] { 1 };
        var hash = new byte[] { 2 };

        _validatorMock
            .Setup(v => v.ValidateAsync(command, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        _passwordManagerMock
            .Setup(p => p.GeneratePasswordParts(command.Password))
            .Returns((salt, hash));

        _storageMock
            .Setup(s => s.AddUserAsync(command.Login, salt, hash, cancellationToken))
            .ThrowsAsync(new UserAlreadyExistsException(command.Login));

        // Act
        Func<Task> act = async () => await _useCase.Execute(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<UserAlreadyExistsException>();
    }
}
