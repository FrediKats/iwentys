using System.Collections.Generic;
using System.Linq;
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
        
        private readonly IGenericRepository<InterestTag> _interestTagRepository;
        private readonly IGenericRepository<StudentInterestTag> _userInterestTagRepository;

        public InterestTagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _interestTagRepository = _unitOfWork.GetRepository<InterestTag>();
            _userInterestTagRepository = _unitOfWork.GetRepository<StudentInterestTag>();
        }

        public async Task<List<InterestTagDto>> GetAllTags()
        {
            List<InterestTag> interestTagEntities = await _interestTagRepository.Get().ToListAsync();
            return interestTagEntities.SelectToList(t => new InterestTagDto(t));
        }

        public async Task<List<InterestTagDto>> GetStudentTags(int studentId)
        {
            return await _userInterestTagRepository
                .Get()
                .Where(ui => ui.StudentId == studentId)
                .Select(ui => ui.InterestTag)
                .Select(InterestTagDto.FromEntity)
                .ToListAsync();
        }

        public async Task AddStudentTag(int studentId, int tagId)
        {
            await _userInterestTagRepository.InsertAsync(new StudentInterestTag {StudentId = studentId, InterestTagId = tagId});
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveStudentTag(int studentId, int tagId)
        {
            _userInterestTagRepository.Delete(new StudentInterestTag { StudentId = studentId, InterestTagId = tagId });
            await _unitOfWork.CommitAsync();
        }
    }
}