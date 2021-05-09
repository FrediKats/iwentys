using System.Threading.Tasks;

namespace Iwentys.Infrastructure.DataAccess
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        Task<int> CommitAsync();
    }
}