using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.UnitofWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> Complete();
}