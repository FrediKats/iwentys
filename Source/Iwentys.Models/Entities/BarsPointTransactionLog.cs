using System;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class BarsPointTransactionLog
    {
        public int Id { get; set; }
        public int FromStudent { get; set; }
        public int ToStudent { get; set; }
        public DateTime TransactionTime { get; set; }
        public int Value { get; set; }

        public BarsPointTransactionStatus Status { get; set; }

        public static BarsPointTransactionLog CompletedFor(Student fromUser, Student toUser, int value)
        {
            return new BarsPointTransactionLog
            {
                FromStudent = fromUser.Id,
                ToStudent = toUser.Id,
                Value = value,
                TransactionTime = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Completed
            };
        }

        public static BarsPointTransactionLog RegisterFail(Student fromUser, Student toUser, int value)
        {
            return new BarsPointTransactionLog
            {
                FromStudent = fromUser.Id,
                ToStudent = toUser.Id,
                Value = value,
                TransactionTime = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Failed
            };
        }
    }
}