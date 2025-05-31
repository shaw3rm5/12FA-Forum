using Forum.Application.Authentication;
using Forum.Application.Authorization;

namespace Forum.Application.UseCases.SignUp;

public class SignOutIntentionManager : IIntentionResolver<SignOutIntention>
{
    public bool IsAllowed(IIdentity subject, SignOutIntention intention)
    {
        return intention switch
        {
            SignOutIntention.Logout => subject.IsAuthenticated(),
            _ => false
        };
    }
}