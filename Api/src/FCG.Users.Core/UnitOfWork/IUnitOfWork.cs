using FCG.Users.Core.Repository;

namespace FCG.Users.Core.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChanges();

    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
}
