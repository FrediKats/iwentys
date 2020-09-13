using FluentResults;
using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IBarsPointTransactionLogService
    {
        Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value);
    }
}