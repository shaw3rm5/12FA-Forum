using Forum.Application.Authentication;
using Forum.Application.Authorization;
using Forum.Application.UseCases.SignUp;

namespace Forum.Application.UseCases.SignOut;

public class SignOutUseCase : ISignOutUseCase
{
    private readonly IIntentionManager _intentionManager;
    private readonly IIdentityProvider _identityProvider;
    private readonly ISignOutStorage _signOutStorage;

    public SignOutUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ISignOutStorage signOutStorage)
    {
        _intentionManager = intentionManager;
        _identityProvider = identityProvider;
        _signOutStorage = signOutStorage;
    }
    
    public async Task Execute(SignOutCommand commnd, CancellationToken cancellationToken)
    {
        _intentionManager.ThrowIfForbidden(SignOutIntention.Logout);

        var sessionId = _identityProvider.Current.SessionId;

        await _signOutStorage.RemoveSession(sessionId, cancellationToken);
    }
}