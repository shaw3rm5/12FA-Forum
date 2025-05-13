using Serilog;
using Serilog.Filters;

namespace FRM.API.Extensions;

public static class LoggerRegistrator
{
    public static void AddLoggerDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(l => l.AddSerilog(
            new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Application", "FRM.API")
                .Enrich.WithProperty("Environment", configuration.GetSection("Environment").Value)
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .WriteTo.OpenSearch(
                        configuration.GetConnectionString("Logs"),
                        "forum-logs-{0:yyyy.MM.dd}"))
                .WriteTo.Logger(lc => lc.WriteTo.Console())
                .CreateLogger()
        ));
    }
}