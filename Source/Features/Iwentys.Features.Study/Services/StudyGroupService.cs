using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
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
        
        private readonly IGenericRepository<StudyGroupEntity> _studyGroupRepository;
        private readonly IGroupSubjectRepository _groupSubjectRepository;

        public StudyGroupService(IGroupSubjectRepository groupSubjectRepository, IUnitOfWork unitOfWork)
        {
            _groupSubjectRepository = groupSubjectRepository;
            _unitOfWork = unitOfWork;
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroupEntity>();

        }

        public async Task<GroupProfileResponseDto> Get(string groupName)
        {
            //TODO: meh
            groupName = new GroupName(groupName).Name;
            StudyGroupEntity studyGroup = await _studyGroupRepository.GetAsync().FirstOrDefaultAsync(s => s.GroupName == groupName);
            return new GroupProfileResponseDto(studyGroup);
        }

        public Task<List<StudyGroupEntity>> GetStudyGroupsForDtoAsync(int? courseId)
        {
            return _groupSubjectRepository.GetStudyGroupsForDto(courseId).ToListAsync();
        }

        //TODO: ensure it's compile to sql
        public async Task<GroupProfileResponseDto> GetStudentGroup(int studentId)
        {
            StudyGroupEntity studyGroupEntity = await _studyGroupRepository.GetAsync().FirstOrDefaultAsync(g => g.Students.Any(s => s.Id == studentId));
            return studyGroupEntity.Maybe(s => new GroupProfileResponseDto(s));
        }
    }
}