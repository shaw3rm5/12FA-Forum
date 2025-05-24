namespace Forum.Application.Authentication;

public interface ISecurityManager
{
    public bool ComparePasswords(string password, string salt, string hash);

    public (string Salt, string Hash) GeneratePasswordParts(string password);
}