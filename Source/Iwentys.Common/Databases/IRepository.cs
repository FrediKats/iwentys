using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;

namespace Iwentys.Common.Databases
{
    public interface IRepository<TEntity>
    {
        public IQueryable<TEntity> Read();
        public Task<TEntity> UpdateAsync(TEntity entity);
    }

    public interface IRepository<TEntity, TKey> : IRepository<TEntity>
    {
        public Task<TEntity> ReadByIdAsync(TKey key);
        public Task<int> DeleteAsync(TKey key);
    }

    public static class RepositoryExtensions
    {
        public static async Task<TEntity> GetAsync<TEntity, TKey>(this IRepository<TEntity, TKey> repository, TKey key)
        {
            TEntity entity = await repository.ReadByIdAsync(key);
            return entity ?? throw EntityNotFoundException.Create(repository.GetType().Name, key);
        }
    }
}