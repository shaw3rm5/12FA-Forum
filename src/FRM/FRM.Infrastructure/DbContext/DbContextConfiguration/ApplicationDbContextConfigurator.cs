using Forum.Infrastructure;
using Forum.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Forum.Infrasctructure.Repository;

public class ApplicationDbContextConfigurator : DbContextOptionsConfigurator<ApplicationDbContext>
{
    protected override string ConnectionString { get; } = "Postgres";
    
    public ApplicationDbContextConfigurator(ILoggerFactory loggerFactory, IConfiguration configuration)
        : base(loggerFactory, configuration) { }

}