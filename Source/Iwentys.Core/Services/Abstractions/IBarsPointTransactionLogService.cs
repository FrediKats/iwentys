using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Tef.BotFramework.Common;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IBarsPointTransactionLogService
    {
        Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value);
    }
}