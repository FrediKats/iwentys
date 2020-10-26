using FluentResults;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;

namespace Iwentys.Core.Services
{
    public class BarsPointTransactionLogService
    {
        private readonly DatabaseAccessor _database;

        public BarsPointTransactionLogService(DatabaseAccessor database)
        {
            _database = database;
        }

        public Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value)
        {
            //TODO: Use transaction for whole method
            StudentEntity from = _database.Student.Get(fromId);
            StudentEntity to = _database.Student.Get(toId);

            Result<BarsPointTransactionLog> transaction;
            if (from.BarsPoints < value)
            {
                transaction = Result.Fail<BarsPointTransactionLog>(new Error("Transfer failed").CausedBy(InnerLogicException.NotEnoughBarsPoints()));
            }
            else
            {
                transaction = Result.Ok(BarsPointTransactionLog.CompletedFor(from, to, value));
                from.BarsPoints -= value;
                to.BarsPoints += value;

                _database.Student.Update(from);
                _database.Student.Update(to);
            }

            _database.BarsPointTransactionLog.Create(transaction.Value);

            return transaction;
        }
    }
}