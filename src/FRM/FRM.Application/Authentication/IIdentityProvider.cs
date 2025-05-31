namespace Forum.Application.Authentication;

public interface IIdentityProvider
{
    IIdentity Current { get; set; }
}
