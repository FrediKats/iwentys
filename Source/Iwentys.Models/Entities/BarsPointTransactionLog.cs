using System;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class BarsPointTransactionLog
    {
        public int Id { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public DateTime TransactionTime { get; set; }
        public int Value { get; set; }

        public BarsPointTransactionStatus Status { get; set; }

        public static BarsPointTransactionLog CompletedFor(UserProfile fromUser, UserProfile toUser, int value)
        {
            return new BarsPointTransactionLog
            {
                FromUser = fromUser.Id,
                ToUser = toUser.Id,
                Value = value,
                TransactionTime = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Completed
            };
        }

        public static BarsPointTransactionLog RegisterFail(UserProfile fromUser, UserProfile toUser, int value)
        {
            return new BarsPointTransactionLog
            {
                FromUser = fromUser.Id,
                ToUser = toUser.Id,
                Value = value,
                TransactionTime = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Failed
            };
        }
    }
}