using FRM.API.Authentication;

namespace FRM.API.Extensions;

public static class ApiDependencies
{
    public static void RegisterApiDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthTokenStorage, AuthTokenStorage>();
    }
}