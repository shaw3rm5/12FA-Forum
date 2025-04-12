using Forum.Application.Authentication;
using Forum.Application.Authorization;

namespace Forum.Application.UseCases.CreateTopic;

public class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention)
    {
        return intention switch
        {
            TopicIntention.Create => subject.IsAuthenticated(),
            _ => false
        };
    }
}