using Iwentys.Common.Databases;
using Iwentys.Features.Gamification.Entities;

namespace Iwentys.Features.Gamification.Services
{
    public class InterestTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<InterestTagEntity> _interestTagRepository;
        private readonly IGenericRepository<UserInterestTagEntity> _userInterestTagRepository;

        public InterestTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _interestTagRepository = _unitOfWork.GetRepository<InterestTagEntity>();
            _userInterestTagRepository = _unitOfWork.GetRepository<UserInterestTagEntity>();
        }
    }
}