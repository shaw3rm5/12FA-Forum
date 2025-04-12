using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Infrastructure.Repository;

// thx Sxxxxxxxx for help

public static class DataAccesRegister
{
    public static IServiceCollection AddDataAccess<TDbContext, TDbContextConfigurator> (this IServiceCollection services)
        where TDbContext : DbContext
        where TDbContextConfigurator : class, IDbContextOptionsConfigurator<TDbContext>
    {

        services
            .AddEntityFrameworkNpgsql()
            .AddDbContextPool<TDbContext>(Configure<TDbContext>);
        
        services
            .AddSingleton<IDbContextOptionsConfigurator<TDbContext>, TDbContextConfigurator>()
            .AddScoped<DbContext>(provider => provider.GetRequiredService<TDbContext>())
            .AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }

    private static void Configure<TDbContext>(IServiceProvider sp, DbContextOptionsBuilder builder)
        where TDbContext : DbContext
    {
        var configurator = sp.GetRequiredService<IDbContextOptionsConfigurator<TDbContext>>();
        configurator.Configure((DbContextOptionsBuilder<TDbContext>) builder);
    }
}