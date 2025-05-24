using System.Security.Cryptography;
using System.Text;

namespace Forum.Application.Authentication;

public class SecurityManager : ISecurityManager
{
    private readonly Lazy<SHA256> _sha256 = new Lazy<SHA256>(SHA256.Create);
    
    public bool ComparePasswords(string password, string salt, string hash)
    {
        var newHash  = ComputeSha(password, salt);
        return string.Equals(
            Encoding.UTF8.GetString(newHash),
            Encoding.UTF8.GetString(Convert.FromBase64String(hash)));
    }

    public (string Salt, string Hash) GeneratePasswordParts(string password)
    {
        var saltLength = 100;
        var buffer = RandomNumberGenerator.GetBytes(saltLength * 4 / 3);
        var base64String = Convert.ToBase64String(buffer);
        var salt = base64String.Length > saltLength
            ? base64String[..saltLength] 
            : base64String;
        var hash = ComputeSha(password, salt);
        return (salt, Convert.ToBase64String(hash));
    }

    private byte[] ComputeSha(string plainText, string salt)
    {
        var buffer = Encoding.UTF8.GetBytes(plainText).Concat(Convert.FromBase64String(salt)).ToArray();

        lock (_sha256)
        {
            return _sha256.Value.ComputeHash(buffer);
        }
    }
}