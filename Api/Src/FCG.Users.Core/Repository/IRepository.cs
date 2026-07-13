using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FCG.Users.Core.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
