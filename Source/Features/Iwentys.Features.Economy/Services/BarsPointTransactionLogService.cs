using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Economy.Entities;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Economy.Services
{
    public class BarsPointTransactionLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IStudentRepository _studentRepository;
        private readonly IGenericRepository<BarsPointTransactionEntity> _barsPointTransactionRepository;

        public BarsPointTransactionLogService(IStudentRepository studentRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = studentRepository;
            _barsPointTransactionRepository = _unitOfWork.GetRepository<BarsPointTransactionEntity>();
        }

        public async Task<BarsPointTransactionEntity> TransferAsync(int fromId, int toId, int pointAmountToTransfer)
        {
            StudentEntity sender = await _studentRepository.GetAsync(fromId);
            StudentEntity receiver = await _studentRepository.GetAsync(toId);

            if (sender.BarsPoints < pointAmountToTransfer)
                throw InnerLogicException.NotEnoughBarsPoints();

            BarsPointTransactionEntity transaction = BarsPointTransactionEntity.CompletedFor(sender, receiver, pointAmountToTransfer);
            sender.BarsPoints -= pointAmountToTransfer;
            receiver.BarsPoints += pointAmountToTransfer;
                    
            await _studentRepository.UpdateAsync(sender);
            await _studentRepository.UpdateAsync(receiver);
            await _barsPointTransactionRepository.InsertAsync(transaction);

            await _unitOfWork.CommitAsync();
            return transaction;
        }

        public async Task TransferFromSystem(int toId, int pointAmountToTransfer)
        {
            //TODO: implement
            StudentEntity receiver = await _studentRepository.GetAsync(toId);

            receiver.BarsPoints += pointAmountToTransfer;

            await _studentRepository.UpdateAsync(receiver);

        }
    }
}