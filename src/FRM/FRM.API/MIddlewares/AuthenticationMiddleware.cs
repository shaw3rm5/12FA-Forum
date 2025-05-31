using Forum.Application.Authentication;
using FRM.API.Authentication;

namespace FRM.API.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IIdentityProvider identityProvider,
        IAuthenticationService authenticationService,
        IAuthTokenStorage authTokenStorage)
    {
        var identity = authTokenStorage.TryExtractToken(context, out var authToken) 
            ? await  authenticationService.AuthenticateAsync(authToken, context.RequestAborted)
            : UserIdentity.Guest;
        
        identityProvider.Current = identity;
    }
}