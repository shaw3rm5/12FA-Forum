namespace Forum.Application.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity subject) => subject.UserId != Guid.Empty;
}