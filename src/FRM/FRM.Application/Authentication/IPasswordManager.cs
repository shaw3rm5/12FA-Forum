namespace Forum.Application.Authentication;

public interface IPasswordManager
{
    public bool ComparePasswords(string password, byte[] salt, byte[] hash);

    public (byte[] Salt, byte[]  Hash) GeneratePasswordParts(string password);
}