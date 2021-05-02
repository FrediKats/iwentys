using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudyService
    {
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<StudyGroup> _studyGroupRepository;
        private readonly IGenericRepository<StudyCourse> _studyCourseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StudyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
            _studyCourseRepository = _unitOfWork.GetRepository<StudyCourse>();
        }

        public async Task<GroupProfileResponseDto> GetStudentStudyGroup(int studentId)
        {
            return await _studentRepository
                .Get()
                .Where(sgm => sgm.Id == studentId)
                .Select(sgm => sgm.Group)
                .Select(GroupProfileResponseDto.FromEntity)
                .SingleOrDefaultAsync();
        }

        public async Task MakeGroupAdmin(AuthorizedUser initiator, int newGroupAdminId)
        {
            IwentysUser initiatorProfile = await _iwentysUserRepository.GetById(initiator.Id);
            Student newGroupAdminProfile = await _studentRepository.GetById(newGroupAdminId);

            StudyGroup studyGroup = StudyGroup.MakeGroupAdmin(initiatorProfile, newGroupAdminProfile);

            _studyGroupRepository.Update(studyGroup);
            await _unitOfWork.CommitAsync();
        }
    }
}