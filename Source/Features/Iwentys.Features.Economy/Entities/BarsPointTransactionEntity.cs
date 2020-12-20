using System;
using Iwentys.Features.Economy.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Economy.Entities
{
    public class BarsPointTransactionEntity
    {
        public int Id { get; set; }
        public int? FromStudent { get; set; }
        public int? ToStudent { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public int Amount { get; set; }

        public BarsPointTransactionStatus Status { get; set; }

        public static BarsPointTransactionEntity CompletedFor(StudentEntity fromUser, StudentEntity toUser, int value)
        {
            fromUser.BarsPoints -= value;
            toUser.BarsPoints += value;

            return new BarsPointTransactionEntity
            {
                FromStudent = fromUser.Id,
                ToStudent = toUser.Id,
                Amount = value,
                CreationTimeUtc = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Completed
            };
        }

        public static BarsPointTransactionEntity RegisterFail(StudentEntity fromUser, StudentEntity toUser, int value)
        {
            return new BarsPointTransactionEntity
            {
                FromStudent = fromUser.Id,
                ToStudent = toUser.Id,
                Amount = value,
                CreationTimeUtc = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Failed
            };
        }

        public static BarsPointTransactionEntity ReceiveFromSystem(StudentEntity toUser, int value)
        {
            toUser.BarsPoints += value;

            return new BarsPointTransactionEntity
            {
                ToStudent = toUser.Id,
                Amount = value,
                CreationTimeUtc = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Completed
            };
        }
    }
}