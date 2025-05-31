namespace Forum.Application.Authentication;

public class UserIdentity : IIdentity
{
    public UserIdentity(Guid userId, Guid sessionId)
    {
        UserId = userId;
        SessionId = sessionId;
    }
    
    public Guid UserId { get; set; }
    
    public Guid SessionId { get; set; }
    
    public static UserIdentity Guest => new (Guid.Empty, Guid.Empty); 
    
}