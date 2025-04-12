using Forum.Application.Authorization;

namespace Forum.Application.Authentication;

public class IntentionManager : IIntentionManager
{
    private readonly IEnumerable<IIntentionResolver> _intentionResolvers;
    private readonly IIdentityProvider _identityProvider;

    public IntentionManager(
        IEnumerable<IIntentionResolver> intentionResolvers,
        IIdentityProvider identityProvider)
    {
        _intentionResolvers = intentionResolvers;
        _identityProvider = identityProvider;
    }
    
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var resolver = _intentionResolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
        return resolver?.IsAllowed(_identityProvider.Current, intention) ?? false;
    }

    public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
    {
        throw new NotImplementedException();
    }
}