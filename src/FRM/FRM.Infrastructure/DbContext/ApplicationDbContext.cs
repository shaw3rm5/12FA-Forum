using Domain.Models;
using Forum.Infrastructure.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
namespace Forum.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Domain.Models.Forum> Forums { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){}
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentConfiguration).Assembly); // add comment configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TopicConfiguration).Assembly); // add comment configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly); // add user configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ForumConfiguration).Assembly); // add forum configuration
    }
    
}
