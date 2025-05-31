using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Forum.Application.Authentication;

public class AesSymmetricEncryptorDecryptor : ISymmetricDecryptor, ISymmetricEncryptor
{
    private const int IvSize = 16;
    private readonly Lazy<Aes> _aes = new(Aes.Create());
    
    public async Task<string> Encrypt(string plainText, byte[] key, CancellationToken cancellationToken)
    {
        var iv = RandomNumberGenerator.GetBytes(IvSize);
        
        using var encryptedStream = new MemoryStream();
        await encryptedStream.WriteAsync(iv, cancellationToken);
        var encryptor = _aes.Value.CreateEncryptor(key, iv);
        await using (var stream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes(plainText), cancellationToken);
        }
        
        return Convert.ToBase64String(encryptedStream.ToArray());
        
    }
    
    public async Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var iv = new Byte[IvSize];
        Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
        using var decryptedStream = new MemoryStream();
        await decryptedStream.WriteAsync(iv, cancellationToken);
        var decryptor = _aes.Value.CreateDecryptor(key, iv);
        await using (var stream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Write))
        {
            await stream.WriteAsync(encryptedBytes.AsMemory(IvSize), cancellationToken);
        }
        return  Convert.ToBase64String(decryptedStream.ToArray());
    }

}