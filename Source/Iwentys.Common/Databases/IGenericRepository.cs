using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;

namespace Iwentys.Common.Databases
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        Task<TEntity> FindByIdAsync<TKey>(TKey id);
        Task<TEntity> InsertAsync(TEntity entity);
        Task InsertAsync(IEnumerable<TEntity> entities);
        void Delete(TEntity entityToDelete);
        void Delete(List<TEntity> entities);
        void Update(List<TEntity> entitiesToUpdate);
        void Update(TEntity entityToUpdate);
    }
    
    public static class GenericRepositoryExtensions
    {
        public static async Task<TEntity> GetByIdAsync<TEntity, TKey>(this IGenericRepository<TEntity> repository, TKey key) where TEntity : class
        {
            return await repository.FindByIdAsync(key) ?? throw EntityNotFoundException.Create(typeof(TEntity), key);
        }
    }
}