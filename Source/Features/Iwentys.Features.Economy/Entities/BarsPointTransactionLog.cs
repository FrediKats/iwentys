using System;
using Iwentys.Features.Economy.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Economy.Entities
{
    public class BarsPointTransactionLog
    {
        public int Id { get; set; }
        public int FromStudent { get; set; }
        public int ToStudent { get; set; }
        public DateTime TransactionTime { get; set; }
        public int Value { get; set; }

        public BarsPointTransactionStatus Status { get; set; }

        public static BarsPointTransactionLog CompletedFor(StudentEntity fromUser, StudentEntity toUser, int value)
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

        public static BarsPointTransactionLog RegisterFail(StudentEntity fromUser, StudentEntity toUser, int value)
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