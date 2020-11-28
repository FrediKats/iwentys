using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;

namespace Iwentys.Features.Economy.Repositories
{
    public interface IBarsPointTransactionLogRepository : IGenericRepository<BarsPointTransactionLog, int>
    {
        Task<BarsPointTransactionLog> CreateAsync(BarsPointTransactionLog entity);
        IQueryable<BarsPointTransactionLog> Read();
        Task<BarsPointTransactionLog> ReadByIdAsync(int key);
        Task<BarsPointTransactionLog> UpdateAsync(BarsPointTransactionLog entity);
        Task<int> DeleteAsync(int key);
    }

}