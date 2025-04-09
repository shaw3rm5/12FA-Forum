

using Forum.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ForumDbContext>(options =>
{
    options
        .UseNpgsql("Host=localhost;Port=5439;Database=forum;Username=postgres;Password=postgres");
});

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();