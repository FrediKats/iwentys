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
        private readonly IUserProfileRepository _userProfileRepository;

        public BarsPointTransactionLogService(IUserProfileRepository userProfileRepository, IBarsPointTransactionLogRepository barsPointTransactionLogRepository)
        {
            _userProfileRepository = userProfileRepository;
            _barsPointTransactionLogRepository = barsPointTransactionLogRepository;
        }


        public Result<BarsPointTransactionLog> Transfer(int fromId, int toId, int value)
        {
            //TODO: Use transaction for whole method
            UserProfile from = _userProfileRepository.Get(fromId);
            UserProfile to = _userProfileRepository.Get(toId);

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

                _userProfileRepository.Update(from);
                _userProfileRepository.Update(to);
            }

            _barsPointTransactionLogRepository.Create(transaction.Value);

            return transaction;
        }
    }
}