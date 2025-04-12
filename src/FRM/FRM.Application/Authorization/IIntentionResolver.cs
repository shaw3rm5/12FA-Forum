using Forum.Application.Authentication;

namespace Forum.Application.Authorization;

public interface IIntentionResolver
{
}


public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}