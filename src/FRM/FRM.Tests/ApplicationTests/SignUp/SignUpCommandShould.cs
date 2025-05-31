using FluentAssertions;
using Forum.Application.UseCases.SignIn;
using Forum.Application.UseCases.SignUp;

namespace FRM.Tests.ApplicationTests.SignUp;

public class SignUpCommandShould
{
    private SignUpCommandValidator _sut = new();

    [Fact]
    public async Task ValidateNormalCommand()
    {
        var command = new SignUpCommand("randomNickname", "randomPassword");
        var result = await _sut.ValidateAsync(command);
        result.Errors.Should().BeEmpty();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var query = new SignInCommand("randomNickname", "randomPassword");
        yield return new object[] { query with { Login = null, Password = null } };
        yield return new object[] { query with { Login = "random", Password = null } };
        yield return new object[] { query with { Login = null, Password = "randomPassword" } };
        yield return new object[] { query with { Login = "", Password = "" } };
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ValidateInvalidCommand(SignUpCommand command)
    {
        var result = await _sut.ValidateAsync(command);
        result.Errors.Should().NotBeEmpty();
    }


}