using System.Diagnostics.CodeAnalysis;

namespace FRM.API.Authentication;

internal class AuthTokenStorage : IAuthTokenStorage
{
    private const string HeaderKey = "FRM-Auth-Token";
    
    public bool TryExtractToken(HttpContext context, [MaybeNullWhen(false)] out string token)
    {
        if (context.Request.Cookies.TryGetValue(HeaderKey, out var value)
            && string.IsNullOrWhiteSpace(value))
        {
            token = value;
            return true;
        }
        token = string.Empty;
        return false;
    }

    public void Store(HttpContext context, string token) => 
        context.Response.Cookies.Append(HeaderKey, token, new CookieOptions
    {
        HttpOnly = true
    });
}