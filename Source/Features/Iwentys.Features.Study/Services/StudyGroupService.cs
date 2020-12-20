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
        
        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<StudyGroupEntity> _studyGroupRepository;

        public StudyGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroupEntity>();
        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
        {
            var studyGroupEntity = await new GroupName(groupName).GetStudyGroup(_studyGroupRepository);
            return new GroupProfileResponseDto(studyGroupEntity);
        }

        public async Task<List<StudyGroupEntity>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            return await _studyGroupRepository
                .GetAsync()
                .WhereIf(courseId, gs => gs.StudyCourseId == courseId)
                .ToListAsync();
        }

        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student.GroupId is null)
                return null;

            var group = await _studyGroupRepository.GetByIdAsync(student.GroupId);
            return new GroupProfileResponseDto(group);
        }
    }
}