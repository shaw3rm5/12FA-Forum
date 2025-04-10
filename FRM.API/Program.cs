using Forum.Domain.UseCases.GetForums;
using Forum.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Postgres");


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGetForumsUseCase, GetForumsUseCase>();
builder.Services.AddDbContextPool<ForumDbContext>(options =>
{
    options
        .UseNpgsql(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();