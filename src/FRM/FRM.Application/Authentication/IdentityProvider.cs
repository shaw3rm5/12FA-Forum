namespace Forum.Application.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current => 
        new UserIdentity(Guid.Parse("f5eefe5c-53ee-4dfa-a8ea-9c0a3e9c4427"));

}