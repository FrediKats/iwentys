using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Economy.Entities;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Economy.Services
{
    public class BarsPointTransactionLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<BarsPointTransaction> _barsPointTransactionRepository;

        public BarsPointTransactionLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _barsPointTransactionRepository = _unitOfWork.GetRepository<BarsPointTransaction>();
        }

        public async Task<BarsPointTransaction> TransferAsync(int fromId, int toId, int pointAmountToTransfer)
        {
            Student sender = await _studentRepository.FindByIdAsync(fromId);
            Student receiver = await _studentRepository.FindByIdAsync(toId);

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
            Student receiver = await _studentRepository.FindByIdAsync(toId);
            BarsPointTransaction transaction = BarsPointTransaction.ReceiveFromSystem(receiver, pointAmountToTransfer);

            await _barsPointTransactionRepository.InsertAsync(transaction);
            _studentRepository.Update(receiver);
            await _unitOfWork.CommitAsync();
            return transaction;
        }
    }
}