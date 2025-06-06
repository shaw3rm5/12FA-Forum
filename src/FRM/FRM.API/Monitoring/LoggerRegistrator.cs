using Serilog;

namespace FRM.API.Monitoring;

public static class LoggerRegistrator
{
    public static void AddLoggerDependency(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        services.AddSerilog(Log.Logger);
    }
}