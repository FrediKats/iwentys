using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudyGroupService
    {
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IGroupSubjectRepository _groupSubjectRepository;

        public StudyGroupService(IStudyGroupRepository studyGroupRepository, IGroupSubjectRepository groupSubjectRepository)
        {
            _studyGroupRepository = studyGroupRepository;
            _groupSubjectRepository = groupSubjectRepository;
        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
        {
            StudyGroupEntity studyGroup = await _studyGroupRepository.ReadByNamePattern(new GroupName(groupName));
            return new GroupProfileResponseDto(studyGroup);
        }

        public Task<List<StudyGroupEntity>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            return _groupSubjectRepository.GetStudyGroupsForDto(courseId).ToListAsync();
        }

        //TODO: ensure it's compile to sql
        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            StudyGroupEntity studyGroupEntity = await _studyGroupRepository.Read().FirstAsync(g => g.Students.Any(s => s.Id == studentId));
            return new GroupProfileResponseDto(studyGroupEntity);
        }
    }
}