namespace Forum.Application.Authorization;

public interface IIntentionManager
{
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct;
    public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct;
}

public static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention)
        where TIntention : struct
    {
        if (!intentionManager.IsAllowed(intention))
            throw new IntentionManagerException();
    }
}