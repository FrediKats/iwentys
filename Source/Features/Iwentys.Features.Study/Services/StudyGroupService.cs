using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudyGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<StudyGroup> _studyGroupRepository;
        private readonly IGenericRepository<StudyGroupMember> _studyGroupMemberRepository;

        public StudyGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
            _studyGroupMemberRepository = _unitOfWork.GetRepository<StudyGroupMember>();
        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
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

        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            Student student = await _studentRepository.GetById(studentId);

            return await _studyGroupMemberRepository
                .Get()
                .Where(sgm => sgm.StudentId == studentId)
                .Select(sgm => sgm.Group)
                .Select(GroupProfileResponseDto.FromEntity)
                .SingleOrDefaultAsync();
        }

        public async Task MakeGroupAdmin(AuthorizedUser initiator, int newGroupAdminId)
        {
            Student initiatorProfile = await _studentRepository.GetById(initiator.Id);
            SystemAdminUser admin = initiatorProfile.EnsureIsAdmin();
            Student newGroupAdminProfile = await _studentRepository.GetById(newGroupAdminId);

            StudyGroup studyGroup = newGroupAdminProfile.GroupMember.Group;

            studyGroup.GroupAdminId = newGroupAdminProfile.Id;

            _studyGroupRepository.Update(studyGroup);
            await _unitOfWork.CommitAsync();
        }
    }
}