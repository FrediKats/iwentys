using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Economy.Entities;

namespace Iwentys.Features.Economy.Repositories
{
    public interface IBarsPointTransactionRepository : IRepository<BarsPointTransactionEntity, int>
    {
        Task<BarsPointTransactionEntity> CreateAsync(BarsPointTransactionEntity entity);
    }
}