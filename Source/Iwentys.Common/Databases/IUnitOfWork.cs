using System.Threading.Tasks;

namespace Iwentys.Common.Databases
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task<int> CommitAsync();
    }
}