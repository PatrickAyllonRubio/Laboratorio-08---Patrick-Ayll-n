using System.Linq.Expressions;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes);  // Agregamos 'includes' para incluir relaciones
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}