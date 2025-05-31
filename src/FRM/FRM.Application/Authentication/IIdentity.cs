namespace Forum.Application.Authentication;

public interface IIdentity
{
    Guid UserId { get; set; }
    Guid SessionId { get; set; }
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity subject) => subject.UserId != Guid.Empty;
}