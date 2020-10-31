using System.Linq;
using System.Threading.Tasks;
using Iwentys.Models.Exceptions;

namespace Iwentys.Database.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> Read();
        Task<TEntity> Update(TEntity entity);
    }

    public interface IGenericRepository<TEntity, TKey> : IGenericRepository<TEntity>
    {
        Task<TEntity> ReadById(TKey key);
        Task<int> Delete(TKey key);
    }

    public static class GenericRepositoryExtensions
    {
        public static TEntity Get<TEntity, TKey>(this IGenericRepository<TEntity, TKey> repository, TKey key)
        {
            //TODO: add async
            return repository.ReadById(key).Result ?? throw EntityNotFoundException.Create(repository.GetType().Name, key);
        }
    }
}