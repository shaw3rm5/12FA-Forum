using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repository;

public interface IDbContextOptionsConfigurator<TDbContext>
    where TDbContext : DbContext
{
    public void Configure(DbContextOptionsBuilder<TDbContext> builder);
}