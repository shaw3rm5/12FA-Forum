using OpenTelemetry.Metrics;

namespace FRM.API.Monitoring;

internal static class MetricsServiceCollectionExtensions
{
    public static IServiceCollection AddApiMetrics(this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddMeter("FRM.Application")
                .AddPrometheusExporter()
                .AddView("http.server.request.duration", 
                    new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = new[] {0, 0.05, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 10}
                }));

        return services;
    }
}