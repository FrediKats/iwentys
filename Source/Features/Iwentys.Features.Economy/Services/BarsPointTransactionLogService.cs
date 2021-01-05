using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Economy.Entities;

namespace Iwentys.Features.Economy.Services
{
    public class BarsPointTransactionLogService
    {
        private readonly IGenericRepository<BarsPointTransaction> _barsPointTransactionRepository;

        private readonly IGenericRepository<IwentysUser> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BarsPointTransactionLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            _barsPointTransactionRepository = _unitOfWork.GetRepository<BarsPointTransaction>();
        }

        public async Task<BarsPointTransaction> Transfer(int fromId, int toId, int pointAmountToTransfer)
        {
            IwentysUser sender = await _studentRepository.FindByIdAsync(fromId);
            IwentysUser receiver = await _studentRepository.FindByIdAsync(toId);

            if (sender.BarsPoints < pointAmountToTransfer)
                throw InnerLogicException.NotEnoughBarsPoints();

            BarsPointTransaction transaction = BarsPointTransaction.CompletedFor(sender, receiver, pointAmountToTransfer);

            _studentRepository.Update(sender);
            _studentRepository.Update(receiver);
            await _barsPointTransactionRepository.InsertAsync(transaction);

            await _unitOfWork.CommitAsync();
            return transaction;
        }

        public async Task<BarsPointTransaction> TransferFromSystem(int toId, int pointAmountToTransfer)
        {
            IwentysUser receiver = await _studentRepository.FindByIdAsync(toId);
            BarsPointTransaction transaction = BarsPointTransaction.ReceiveFromSystem(receiver, pointAmountToTransfer);

            await _barsPointTransactionRepository.InsertAsync(transaction);
            _studentRepository.Update(receiver);
            await _unitOfWork.CommitAsync();
            return transaction;
        }
    }
}