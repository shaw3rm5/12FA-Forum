using Forum.Infrasctructure.Repository;
using Forum.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static void AddInfrastructureDependencies(this IServiceCollection service)
    {
        service
            .AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>();
        // loading...
    }
}