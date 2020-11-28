using System.Threading.Tasks;
using FluentResults;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Economy.Repositories;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Models.Entities;

namespace Iwentys.Core.Services
{
    public class BarsPointTransactionLogService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IBarsPointTransactionLogRepository _barsPointTransactionLogRepository;

        public BarsPointTransactionLogService(IStudentRepository studentRepository, IBarsPointTransactionLogRepository barsPointTransactionLogRepository)
        {
            _studentRepository = studentRepository;
            _barsPointTransactionLogRepository = barsPointTransactionLogRepository;
        }

        public async Task<Result<BarsPointTransactionLog>> Transfer(int fromId, int toId, int value)
        {
            //TODO: Use transaction for whole method
            StudentEntity from = await _studentRepository.GetAsync(fromId);
            StudentEntity to = await _studentRepository.GetAsync(toId);

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
                    
                await _studentRepository.UpdateAsync(@from);
                await _studentRepository.UpdateAsync(to);
            }

            await _barsPointTransactionLogRepository.CreateAsync(transaction.Value);

            return transaction;
        }
    }
}