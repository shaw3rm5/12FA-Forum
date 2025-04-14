namespace Forum.Application.Authentication;

public interface IIdentity
{
    Guid UserId { get; set; }
}

public class User : IIdentity
{
    public User(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; set; }
    
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity subject) => subject.UserId == Guid.Parse("f5eefe5c-53ee-4dfa-a8ea-9c0a3e9c4427");
}