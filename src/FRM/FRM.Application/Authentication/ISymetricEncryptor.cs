namespace Forum.Application.Authentication;

public interface ISymmetricEncryptor 
{
    public Task<string> Encrypt(string plainText, byte[] key, CancellationToken cancellationToken);   
}