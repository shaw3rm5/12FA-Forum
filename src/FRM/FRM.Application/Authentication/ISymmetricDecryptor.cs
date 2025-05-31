namespace Forum.Application.Authentication;

public interface ISymmetricDecryptor
{
    public Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken);
}