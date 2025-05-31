using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.UseCases.SignOut;
using Moq;
using Moq.Language.Flow;

namespace FRM.Tests.ApplicationTests.SignOut;

public class SignOutUseCaseShould
{
    private readonly IIdentityProvider _provider;
    private readonly SignOutUseCase _sut;
    private readonly ISetup<ISignOutStorage, Task> removeSessionSetup;
    private readonly ISetup<IIdentityProvider, IIdentity> identitySetup;
    private readonly Mock<ISignOutStorage> signOutStorage;
    private readonly Mock<IIntentionManager> identityProvider;

    public SignOutUseCaseShould()
    {
        signOutStorage = new Mock<ISignOutStorage>();
        removeSessionSetup = signOutStorage.Setup(s => s.RemoveSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        var identity = new Mock<IIdentityProvider>();
        identitySetup = identity.Setup(p => p.Current);
        _sut = new SignOutUseCase(
            identityProvider?.Object,
            identity.Object,
            signOutStorage.Object
            );
    }

    [Fact]
    public async Task RemoveCurrentIdentitySession()
    {
        var sessionid = Guid.Parse("c0498a4f-2b9c-487f-b85a-a7f446526c00");
        identitySetup.Returns(new UserIdentity(Guid.Empty, sessionid));
        removeSessionSetup.Returns(Task.CompletedTask);
        await _sut.Execute(new SignOutCommand(), CancellationToken.None);
        
        signOutStorage.Verify(s => s.RemoveSession(sessionid, It.IsAny<CancellationToken>()), Times.Once);
        
        signOutStorage.VerifyNoOtherCalls();
        
    }
}