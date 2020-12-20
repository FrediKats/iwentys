using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<InterestTagDto>> GetAllTags()
        {
            List<InterestTagEntity> interestTagEntities = await _interestTagRepository.GetAsync().ToListAsync();
            return interestTagEntities.SelectToList(t => new InterestTagDto(t));
        }
    }
}