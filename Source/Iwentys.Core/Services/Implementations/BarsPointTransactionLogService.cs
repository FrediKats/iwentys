using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Tef.BotFramework.Common;

namespace Iwentys.Core.Services.Implementations
{
    public class BarsPointTransactionLogService : IBarsPointTransactionLogService
    {
        private readonly IBarsPointTransactionLogRepository _barsPointTransactionLogRepository;
        private readonly IStudentRepository _studentRepository;

        public BarsPointTransactionLogService(IStudentRepository studentRepository, IBarsPointTransactionLogRepository barsPointTransactionLogRepository)
        {
            _studentRepository = studentRepository;
            _barsPointTransactionLogRepository = barsPointTransactionLogRepository;
        }

        public Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value)
        {
            //TODO: Use transaction for whole method
            StudentEntity from = _studentRepository.Get(fromId);
            StudentEntity to = _studentRepository.Get(toId);

            Result<BarsPointTransactionLog> transaction;
            if (from.BarsPoints < value)
            {
                transaction = Result<BarsPointTransactionLog>.Fail(InnerLogicException.NotEnoughBarsPoints().Message, InnerLogicException.NotEnoughBarsPoints());
            }
            else
            {
                transaction = Result<BarsPointTransactionLog>.Ok(BarsPointTransactionLog.CompletedFor(from, to, value));
                from.BarsPoints -= value;
                to.BarsPoints += value;

                _studentRepository.Update(from);
                _studentRepository.Update(to);
            }

            _barsPointTransactionLogRepository.Create(transaction.Value);

            return transaction;
        }
    }
}