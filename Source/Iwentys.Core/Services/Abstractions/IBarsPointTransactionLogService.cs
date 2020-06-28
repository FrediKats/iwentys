using Iwentys.Models.Entities;
using Iwentys.Models.Tools;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IBarsPointTransactionLogService
    {
        Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value);
    }
}