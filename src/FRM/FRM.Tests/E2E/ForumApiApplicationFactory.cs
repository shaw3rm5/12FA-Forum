using Forum.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace FRM.Tests.E2E;

public class ForumApiApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    
    private readonly  PostgreSqlContainer _builder =  new PostgreSqlBuilder().Build();
    
    public async Task InitializeAsync()
    {
        await _builder.StartAsync();
        var applicationDbContext = new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(_builder.GetConnectionString()).Options); 
        await applicationDbContext.Database.MigrateAsync();
        
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new  Dictionary<string, string>
        {
            ["ConnectionStrings:Postgres"] = _builder.GetConnectionString()
        }!).Build(); ;
        
        builder.UseConfiguration(configuration);
    }

    public new async Task DisposeAsync() => await _builder.DisposeAsync();
    
}