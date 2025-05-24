using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Forum.Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationStorage _authenticationStorage;
    private readonly ISecurityManager _securityManager;
    private readonly AuthenticationConfiguration _configurationOptions;
    private readonly Lazy<TripleDES> _tripleDes = new(TripleDES.Create);
    
    public AuthenticationService(
        IAuthenticationStorage authenticationStorage,
        ISecurityManager securityManager,
        IOptions<AuthenticationConfiguration> configurationOptions)
    {
        _authenticationStorage = authenticationStorage;
        _securityManager = securityManager;
        _configurationOptions = configurationOptions.Value;
        _tripleDes.Value.Mode = CipherMode.CBC;
        _tripleDes.Value.Padding = PaddingMode.PKCS7;
    }
    
    public async Task<(bool isSucces, string authToken)> SignIn(BasicSignInCredentials credentials, CancellationToken cancellationToken)
    {
        var recognisedUser = await _authenticationStorage.FindUser(credentials.Login, cancellationToken);
        if (recognisedUser is null)
            throw new Exception("User not found");
       

        var success = _securityManager.ComparePasswords(credentials.Password, recognisedUser.Salt, recognisedUser.PasswordHash);
        var userIdBytes = recognisedUser.UserId.ToByteArray();
        
        using var encryptedStream = new MemoryStream();
        var key = Convert.FromBase64String(_configurationOptions.Key);
        var iv = Convert.FromBase64String(_configurationOptions.Iv);
        await using (var stream = new CryptoStream(
                         encryptedStream,
                         _tripleDes.Value.CreateEncryptor(key, iv),
                         CryptoStreamMode.Write))
        {
            await stream.WriteAsync(userIdBytes, cancellationToken);
        } 
        return (success, Convert.ToBase64String(encryptedStream.ToArray()));
    }

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        var decryptedStream = new MemoryStream();
        var key =  Convert.FromBase64String(_configurationOptions.Key);
        var iv = Convert.FromBase64String(_configurationOptions.Iv);
        
        await using (var stream = new CryptoStream(
                         decryptedStream,
                         _tripleDes.Value.CreateDecryptor(key, iv),
                         CryptoStreamMode.Write))
        {
            var encryptedBytes = Convert.FromBase64String(authToken);
            await stream.WriteAsync(encryptedBytes, cancellationToken);
        }
        
        var userId =  new Guid(decryptedStream.ToArray());
        
        return new UserIdentity(userId);
    }
}