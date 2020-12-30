using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudyGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<StudyGroup> _studyGroupRepository;

        public StudyGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
        {
            var studyGroupEntity = await new GroupName(groupName).GetStudyGroup(_studyGroupRepository);
            return new GroupProfileResponseDto(studyGroupEntity);
        }

        public async Task<List<StudyGroup>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            return await _studyGroupRepository
                .Get()
                .WhereIf(courseId, gs => gs.StudyCourseId == courseId)
                .ToListAsync();
        }

        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            var student = await _studentRepository.FindByIdAsync(studentId);
            if (student.GroupId is null)
                return null;

            var group = await _studyGroupRepository.FindByIdAsync(student.GroupId);
            return new GroupProfileResponseDto(group);
        }
    }
}