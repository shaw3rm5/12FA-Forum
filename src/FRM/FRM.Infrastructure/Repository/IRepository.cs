using System.Linq.Expressions;

namespace Forum.Infrastructure.Repository;

public interface IRepository<TEntity>
    where TEntity : class
{
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    public Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);
    
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    
    public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector);
    
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    
    public IQueryable<TEntity> AsQueryable();
}