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

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<BarsPointTransactionEntity> _barsPointTransactionRepository;

        public BarsPointTransactionLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _barsPointTransactionRepository = _unitOfWork.GetRepository<BarsPointTransactionEntity>();
        }

        public async Task<BarsPointTransactionEntity> TransferAsync(int fromId, int toId, int pointAmountToTransfer)
        {
            StudentEntity sender = await _studentRepository.GetByIdAsync(fromId);
            StudentEntity receiver = await _studentRepository.GetByIdAsync(toId);

            if (sender.BarsPoints < pointAmountToTransfer)
                throw InnerLogicException.NotEnoughBarsPoints();

            BarsPointTransactionEntity transaction = BarsPointTransactionEntity.CompletedFor(sender, receiver, pointAmountToTransfer);
                    
            _studentRepository.Update(sender);
            _studentRepository.Update(receiver);
            await _barsPointTransactionRepository.InsertAsync(transaction);

            await _unitOfWork.CommitAsync();
            return transaction;
        }

        public async Task<BarsPointTransactionEntity> TransferFromSystem(int toId, int pointAmountToTransfer)
        {
            StudentEntity receiver = await _studentRepository.GetByIdAsync(toId);
            BarsPointTransactionEntity transaction = BarsPointTransactionEntity.ReceiveFromSystem(receiver, pointAmountToTransfer);

            await _barsPointTransactionRepository.InsertAsync(transaction);
            _studentRepository.Update(receiver);
            await _unitOfWork.CommitAsync();
            return transaction;
        }
    }
}