using Forum.Domain;
using Forum.Domain.UseCases.CreateTopic;
using Forum.Domain.UseCases.GetForums;
using Forum.Infrastructure;
using Forum.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
builder.Services.AddTransient<IGuidFactory, GuidFactory>();
builder.Services.AddTransient<IMomentProvider, MomentProvider>();
builder.Services.AddScoped<ICreateTopicUseCase, CreateTopicUseCase>();

builder.Services.AddDataAccess<ApplicationDbContext, ApplicationDbContextConfigurator>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();