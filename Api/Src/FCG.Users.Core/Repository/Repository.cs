using FCG.Users.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq.Expressions;

namespace FCG.Users.Core.Repository;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        DbContext = context;
        _dbSet = DbContext.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<TEntity> GetByAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.SetUpdated();
        }

        _dbSet.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.Delete();
        }
    }
}

