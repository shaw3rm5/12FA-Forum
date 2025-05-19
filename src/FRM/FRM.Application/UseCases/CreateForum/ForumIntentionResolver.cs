using Forum.Application.Authentication;
using Forum.Application.Authorization;

namespace Forum.Application.UseCases.CreateForum;

public class ForumIntentionResolver : IIntentionResolver<ForumIntention>
{
    public bool IsAllowed(IIdentity subject, ForumIntention intention)
    {
        return intention switch
        {
            ForumIntention.Create => subject.IsAuthenticated(),
            _ => false
        };
    }
}