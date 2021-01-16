using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
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

        public async Task<List<StudyCourseInfoDto>> GetStudyCourses()
        {
            return await _studyCourseRepository
                .Get()
                .Select(StudyCourseInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<GroupProfileResponseDto> GetStudyGroup(string groupName)
        {
            var name = new GroupName(groupName);
            List<GroupProfileResponseDto> result = await _studyGroupRepository
                .Get()
                .Where(StudyGroup.IsMatch(name))
                .Select(GroupProfileResponseDto.FromEntity)
                .ToListAsync();

            return result.Single();
        }

        public async Task<List<GroupProfileResponseDto>> GetStudyGroupsForDto(int? courseId)
        {
            List<GroupProfileResponseDto> result = await _studyGroupRepository
                .Get()
                .WhereIf(courseId, gs => gs.StudyCourseId == courseId)
                .Select(GroupProfileResponseDto.FromEntity)
                .ToListAsync();

            return result;
        }

        public async Task<GroupProfileResponseDto> GetStudentStudyGroup(int studentId)
        {
            Student student = await _studentRepository.GetById(studentId);

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
            SystemAdminUser admin = initiatorProfile.EnsureIsAdmin();
            Student newGroupAdminProfile = await _studentRepository.GetById(newGroupAdminId);

            StudyGroup studyGroup = newGroupAdminProfile.Group;

            studyGroup.GroupAdminId = newGroupAdminProfile.Id;

            _studyGroupRepository.Update(studyGroup);
            await _unitOfWork.CommitAsync();
        }
    }
}