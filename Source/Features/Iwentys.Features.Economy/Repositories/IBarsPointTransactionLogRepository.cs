using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;

namespace Iwentys.Features.Economy.Repositories
{
    public interface IBarsPointTransactionLogRepository : IGenericRepository<BarsPointTransactionLog, int>
    {
        Task<BarsPointTransactionLog> CreateAsync(BarsPointTransactionLog entity);
    }
}