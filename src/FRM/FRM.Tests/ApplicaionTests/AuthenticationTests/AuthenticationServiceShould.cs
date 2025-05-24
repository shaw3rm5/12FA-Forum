using FluentAssertions;
using Forum.Application.Authentication;
using Forum.Application.Storage;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace FRM.Tests.ApplicaionTests.AuthenticationTests;

public class AuthenticationServiceShould
{
    private readonly string passwordHash =
        "-ybkUfkH8g01iyhhGgbBZtdZGSHVHeojcHg1DthB9WemLwhSZmVD3gTtTWyLxY_AcYJMiRxxTjoDAmhPcOlj2HA-QNC3Rj1uaPKWy2c0S27ILkc0FUd06p5EINv2okjA0Hp7T85VYiGiPanrXHf8b8VG12q_YzmdFiY97OSYt35IKaIiFR16QAl0HK8WMZ9DX_dKh1s8rP9QBuGPEplZZfbir2Dk0du3SxN7JU3ANa3RmQbb9Eo82RMU8cOfVuHx1cvy4WrMDM0hgh6Mh-m6CJqhjimMctV6SgiVue20dOa_z1XbibZgA50OPZFqXATfr61a_EsYlT9eU-eJA7G_";
    private readonly ISetup<IAuthenticationStorage, Task<RecognisedUser?>> _findUserSetup;
    private readonly AuthenticationService _sut;
    private readonly Mock<IOptions<AuthenticationConfiguration>> _configuration;
    private readonly Mock<IAuthenticationStorage> _storage;

    public AuthenticationServiceShould()
    {
        _storage = new Mock<IAuthenticationStorage>();
        _findUserSetup = _storage.Setup(r => r.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        var securityManager = new Mock<ISecurityManager>();
        securityManager
            .Setup(r => r.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        _configuration = new Mock<IOptions<AuthenticationConfiguration>>();
        
        _configuration
            .Setup(r => r.Value)
            .Returns(new AuthenticationConfiguration()
            {
                Key = "1dMZHqKPdqAGSBZoL9LixVpvJKe0nOag",
                Iv = "v3n3Nn8k5eU="
            });
        
        _sut = new AuthenticationService(_storage.Object, securityManager.Object, _configuration.Object);
    }

    [Fact]
    public async Task ReturnSuccess_WhenUserFound()
    {
        _findUserSetup.ReturnsAsync
            (new RecognisedUser
            {
                UserId = Guid.Parse("4b8f424b-d6d0-47a7-a563-c5c260f874eb"),
                Salt = "ZDIuuxubJcFN8ZRF66fpPQ==",
                PasswordHash = passwordHash
            });

    var (success, authToken) = await _sut.SignIn(new BasicSignInCredentials("User", "Password"), CancellationToken.None);
        success.Should().BeTrue();
        authToken.Should().NotBeEmpty();
    }


    [Fact]
    public async Task AuthenticateUser_AfterTheySignIn()
    {

        var userId = Guid.Parse("b31ae3f5-ef2a-4408-b338-f1b29984c087");
        _findUserSetup.ReturnsAsync
            (new RecognisedUser { UserId = userId});

    var (_, authToken) = await _sut.SignIn(new BasicSignInCredentials("User", "Password"), CancellationToken.None);
        
        var identity = await _sut.Authenticate(authToken, CancellationToken.None);
        identity.UserId.Should().Be(userId);
    }


    [Fact]
    public async Task SignInUser_WhenPasswordMatches()
    {
        var password = "gondon";
        var securityManager = new SecurityManager();
        var (salt, hash) = securityManager.GeneratePasswordParts(password);
        
        var localSut = new AuthenticationService(_storage.Object,  securityManager, _configuration.Object);
        
        _findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = Guid.Parse("990af2f6-13d4-4caa-aaf1-fe1aeaffc671"),
            Salt = salt,
            PasswordHash = hash
        });
        
        var (success, _) = await localSut.SignIn(new BasicSignInCredentials("User", password), CancellationToken.None);
        success.Should().BeTrue();


    }
}