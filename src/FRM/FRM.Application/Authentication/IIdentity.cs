namespace Forum.Application.Authentication;

public interface IIdentity
{
    Guid UserId { get; set; }
}

public class UserIdentity : IIdentity
{
    public UserIdentity(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; set; }
    
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity subject) => subject.UserId != Guid.Empty;
}