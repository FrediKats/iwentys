using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IGroupSubjectRepository _groupSubjectRepository;

        public SubjectService(ISubjectRepository subjectRepository, IGroupSubjectRepository groupSubjectRepository)
        {
            _subjectRepository = subjectRepository;
            _groupSubjectRepository = groupSubjectRepository;
        }

        public async Task<List<SubjectProfileDto>> Get()
        {
            List<SubjectEntity> subjects = await _subjectRepository
                .Read()
                .ToListAsync();

            return subjects.SelectToList(entity => new SubjectProfileDto(entity));

        }

        public async Task<SubjectProfileDto> Get(int id)
        {
            SubjectEntity subject = await _subjectRepository
                .Read()
                .FirstAsync(s => s.Id == id);

            return new SubjectProfileDto(subject);
        }

        public async Task<List<SubjectProfileDto>> GetGroupSubjects(int groupId)
        {
            List<SubjectEntity> subjectEntities = await _groupSubjectRepository
                .GetSubjectsForDto(StudySearchParametersDto.ForGroup(groupId))
                .ToListAsync();

            return subjectEntities.SelectToList(entity => new SubjectProfileDto(entity));
        }

        public Task<List<SubjectEntity>> GetSubjectsForDtoAsync(StudySearchParametersDto searchParametersDto)
        {
            return _groupSubjectRepository.GetSubjectsForDto(searchParametersDto).ToListAsync();
        }
    }
}