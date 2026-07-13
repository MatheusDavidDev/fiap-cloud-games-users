using FCG.Users.Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace FCG.Users.Core.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private bool _disposed = false;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // clear repositories
                if (_repositories != null)
                {
                    _repositories.Clear();
                }

                // dispose the db context.
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories == null)
        {
            _repositories = new Dictionary<Type, object>();
        }


        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new Repository<TEntity>(_context);
        }

        return (IRepository<TEntity>)_repositories[type];
    }
}

