using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Students.Entities;
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

        public StudyGroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
        {
            var name = new GroupName(groupName);
            List<GroupProfileResponseDto> result = await _studyGroupRepository
                .Get()
                .Where(StudyGroup.IsMatch(name))
                .Select(GroupProfileResponseDto.FromEntity)
                .WithStudents(_studentRepository);

            return result.Single();
        }

        public async Task<List<GroupProfileResponseDto>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            List<GroupProfileResponseDto> result = await _studyGroupRepository
                .Get()
                .WhereIf(courseId, gs => gs.StudyCourseId == courseId)
                .Select(GroupProfileResponseDto.FromEntity)
                .WithStudents(_studentRepository);

            return result;
        }

        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            Student student = await _studentRepository.GetByIdAsync(studentId);
            if (student.GroupId is null)
                return null;

            List<GroupProfileResponseDto> result = await _studyGroupRepository
                .Get()
                .Where(sg => sg.Id == student.GroupId)
                .Select(GroupProfileResponseDto.FromEntity)
                .WithStudents(_studentRepository);

            return result.Single();
        }
    }
}