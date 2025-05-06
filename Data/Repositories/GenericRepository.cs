using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly LinqExampleContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(LinqExampleContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Modificado para aceptar parámetros de tipo Expression<Func<TEntity, object>>[] para incluir relaciones
        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includes) // Agregamos parámetros de "include"
        {
            IQueryable<TEntity> query = _dbSet;

            // Aplicamos el Include para cada propiedad relacionada pasada como parámetro
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Aplicamos el filtro y ejecutamos la consulta
            return await query.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}