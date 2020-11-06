using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;

namespace Iwentys.Common.Tools
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> Read();
        Task<TEntity> UpdateAsync(TEntity entity);
    }

    public interface IGenericRepository<TEntity, TKey> : IGenericRepository<TEntity>
    {
        Task<TEntity> ReadByIdAsync(TKey key);
        Task<int> DeleteAsync(TKey key);
    }

    public static class GenericRepositoryExtensions
    {
        public static async Task<TEntity> GetAsync<TEntity, TKey>(this IGenericRepository<TEntity, TKey> repository, TKey key)
        {
            TEntity entity = await repository.ReadByIdAsync(key);
            return entity ?? throw EntityNotFoundException.Create(repository.GetType().Name, key);
        }
    }
}