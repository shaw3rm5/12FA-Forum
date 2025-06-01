using Forum.Application;
using Forum.Application.Authentication;
using Forum.Infrastructure;
using FRM.API.Extensions;
using FRM.API.Middlewares;
using FRM.API.Monitoring;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetRequiredSection("Authentication").Bind);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddApiMetrics();
builder.Services.AddLoggerDependency(builder.Configuration);
builder.Services.RegisterApiDependencies();
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies();

var app = builder.Build();

app.MapControllers();
app.MapPrometheusScrapingEndpoint();

app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public partial class Program { } 
