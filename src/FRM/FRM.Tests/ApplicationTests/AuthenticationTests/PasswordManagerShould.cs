using System.Security.Cryptography;
using FluentAssertions;
using Forum.Application.Authentication;
using Xunit.Abstractions;

namespace FRM.Tests.ApplicationTests.AuthenticationTests;

public class PasswordManagerShould
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly PasswordManager _passwordManager = new(); 
    private static byte[] emptySalt = Enumerable.Repeat<byte>(0, 100).ToArray();
    private static byte[] emptyHash = Enumerable.Repeat<byte>(0, 32).ToArray();

    public PasswordManagerShould(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    
    [Theory]
    [InlineData("password")]
    [InlineData("qwerty12")]
    public void GenerateMeaningfulSaltAndHash(string password)
    {
        var (salt, hash) = _passwordManager.GeneratePasswordParts(password);
        salt.Should().HaveCount(100).And.NotBeEquivalentTo(emptySalt);
        hash.Should().HaveCount(32).And.NotBeEquivalentTo(emptyHash);
        
        _passwordManager.ComparePasswords(password, salt, hash).Should().BeTrue();
    }
    
    // PdiAw9FvfRGHpWT3iaNjE4GTWY+z4+UDlzLUSh3Yceg=

}