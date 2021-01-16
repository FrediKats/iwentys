using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.InterestTags.Entities;
using Iwentys.Features.InterestTags.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.InterestTags.Services
{
    public class InterestTagService
    {
        private readonly IGenericRepository<InterestTag> _interestTagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserInterestTag> _userInterestTagRepository;

        public InterestTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _interestTagRepository = _unitOfWork.GetRepository<InterestTag>();
            _userInterestTagRepository = _unitOfWork.GetRepository<UserInterestTag>();
        }

        public async Task<List<InterestTagDto>> GetAllTags()
        {
            List<InterestTag> interestTagEntities = await _interestTagRepository.Get().ToListAsync();
            return interestTagEntities.SelectToList(t => new InterestTagDto(t));
        }

        public async Task<List<InterestTagDto>> GetUserTags(int userId)
        {
            return await _userInterestTagRepository
                .Get()
                .Where(ui => ui.UserId == userId)
                .Select(ui => ui.InterestTag)
                .Select(InterestTagDto.FromEntity)
                .ToListAsync();
        }

        public async Task AddUserTag(int userId, int tagId)
        {
            await _userInterestTagRepository.InsertAsync(new UserInterestTag {UserId = userId, InterestTagId = tagId});
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveUserTag(int userId, int tagId)
        {
            _userInterestTagRepository.Delete(new UserInterestTag {UserId = userId, InterestTagId = tagId});
            await _unitOfWork.CommitAsync();
        }
    }
}