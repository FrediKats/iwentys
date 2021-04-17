using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.InterestTags.Services
{
    public class InterestTagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserInterestTag> _userInterestTagRepository;

        public InterestTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userInterestTagRepository = _unitOfWork.GetRepository<UserInterestTag>();
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
    }
}