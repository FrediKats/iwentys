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

        public StudyGroupService(IStudyGroupRepository studyGroupRepository)
        {
            _studyGroupRepository = studyGroupRepository;
        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
        {
            StudyGroupEntity studyGroup = await _studyGroupRepository.ReadByNamePattern(new GroupName(groupName));
            return new GroupProfileResponseDto(studyGroup);
        }

        //TODO: ensure it's compile to sql
        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            StudyGroupEntity studyGroupEntity = await _studyGroupRepository.Read().FirstAsync(g => g.Students.Any(s => s.Id == studentId));
            return new GroupProfileResponseDto(studyGroupEntity);
        }
    }
}