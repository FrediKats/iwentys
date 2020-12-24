using System.Linq;
using System.Threading.Tasks;

namespace Iwentys.Common.Databases
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAsync();
        Task<TEntity> GetByIdAsync<TKey>(TKey id);
        Task<TEntity> InsertAsync(TEntity entity);
        Task DeleteAsync<TKey>(TKey id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
    }
}