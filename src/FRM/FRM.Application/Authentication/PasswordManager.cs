using System.Security.Cryptography;
using System.Text;

namespace Forum.Application.Authentication;

public class PasswordManager : IPasswordManager
{
    private const int SaltLength = 100;
    private readonly Lazy<SHA256> _sha256 = new Lazy<SHA256>(SHA256.Create);
    
    public bool ComparePasswords(string password, byte[] salt, byte[] hash)
    {
        var newHash = ComputeHash(password, salt);
        return newHash.SequenceEqual(hash);
    }

    public (byte[] Salt, byte[] Hash) GeneratePasswordParts(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);
        var hash = ComputeHash(password, salt);
        return (salt, hash.ToArray());
    }

    private ReadOnlySpan<byte> ComputeHash(string plainText, byte[] salt)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        var buffer = new byte[plainTextBytes.Length + salt.Length];
        Array.Copy(plainTextBytes, buffer, plainTextBytes.Length);
        Array.Copy(salt, 0, buffer, plainTextBytes.Length, salt.Length);
        
        
        byte[] hash;
        lock (_sha256)
        {
            hash =  _sha256.Value.ComputeHash(buffer);
        }

        return hash;
    }
}