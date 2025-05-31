using Domain.Models;
using Forum.Infrastructure.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
namespace Forum.Infrastructure;

public class ApplicationDbContext : DbContext
{
    private DbSet<Comment> Comments;
    private DbSet<Domain.Models.Forum> Forums;
    private DbSet<Topic> Topics;
    private DbSet<User> Users;
    private DbSet<Session> Sessions;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentConfiguration).Assembly); // add comment configuration
    }
}
