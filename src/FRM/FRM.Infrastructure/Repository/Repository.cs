using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Forum.Infrastructure.Repository;

public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<TEntity> DbSet;
    
    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        DbSet = _dbContext.Set<TEntity>();
    }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbContext.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbContext.Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);
        foreach (var entity in entities)
        {
            _dbContext.Remove(entity);
        }
        await SaveChangesAsync(cancellationToken);
    }
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return DbSet.AsQueryable<TEntity>();
    }

    private async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}