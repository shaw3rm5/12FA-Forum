using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Forum.Infrastructure.Repository;

public abstract class DbContextOptionsConfigurator<TDbContext> : IDbContextOptionsConfigurator<TDbContext> where TDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;
    
    protected abstract string ConnectionString { get; } 

    public DbContextOptionsConfigurator(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _loggerFactory = loggerFactory;
        _configuration = configuration;
    }
    
    public void Configure(DbContextOptionsBuilder<TDbContext> builder)
    {
        var connectionString = _configuration.GetConnectionString(ConnectionString);
        if(string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Connection string {connectionString} not found");
        builder
            .UseLoggerFactory(_loggerFactory)
            .UseNpgsql(connectionString, x => x.CommandTimeout(60));
    }
}