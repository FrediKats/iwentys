using System;
using Iwentys.Features.Economy.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Economy.Entities
{
    public class BarsPointTransaction
    {
        public int Id { get; init; }
        public int? FromStudent { get; init; }
        public int? ToStudent { get; init; }
        public DateTime CreationTimeUtc { get; init; }
        public int Amount { get; init; }

        public BarsPointTransactionStatus Status { get; init; }

        public static BarsPointTransaction CompletedFor(Student fromUser, Student toUser, int value)
        {
            fromUser.BarsPoints -= value;
            toUser.BarsPoints += value;

            return new BarsPointTransaction
            {
                FromStudent = fromUser.Id,
                ToStudent = toUser.Id,
                Amount = value,
                CreationTimeUtc = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Completed
            };
        }

        public static BarsPointTransaction RegisterFail(Student fromUser, Student toUser, int value)
        {
            return new BarsPointTransaction
            {
                FromStudent = fromUser.Id,
                ToStudent = toUser.Id,
                Amount = value,
                CreationTimeUtc = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Failed
            };
        }

        public static BarsPointTransaction ReceiveFromSystem(Student toUser, int value)
        {
            toUser.BarsPoints += value;

            return new BarsPointTransaction
            {
                ToStudent = toUser.Id,
                Amount = value,
                CreationTimeUtc = DateTime.UtcNow,
                Status = BarsPointTransactionStatus.Completed
            };
        }
    }
}