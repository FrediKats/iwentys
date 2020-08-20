using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;

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
            Student from = _studentRepository.Get(fromId);
            Student to = _studentRepository.Get(toId);

            Result<BarsPointTransactionLog> transaction;
            if (from.BarsPoints < value)
            {
                transaction = Result.From(BarsPointTransactionLog.RegisterFail(from, to, value), InnerLogicException.NotEnoughBarsPoints().Message);
            }
            else
            {
                transaction = Result.From(BarsPointTransactionLog.CompletedFor(from, to, value));
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