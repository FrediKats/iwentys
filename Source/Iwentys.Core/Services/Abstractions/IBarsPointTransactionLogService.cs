using Iwentys.Models.Entities;
using Tef.BotFramework.Common;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IBarsPointTransactionLogService
    {
        Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value);
    }
}