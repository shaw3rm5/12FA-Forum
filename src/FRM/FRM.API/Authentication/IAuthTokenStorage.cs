namespace FRM.API.Authentication;

public interface IAuthTokenStorage
{
    public bool TryExtractToken(HttpContext context, out string token);
    public void Store(HttpContext context, string token);
}