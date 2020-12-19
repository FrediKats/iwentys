using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class SubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<SubjectEntity> _subjectRepository;
        private readonly IGenericRepository<GroupSubjectEntity> _groupSubjectRepository;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _subjectRepository = _unitOfWork.GetRepository<SubjectEntity>();
            _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubjectEntity>();
        }

        public async Task<List<SubjectProfileDto>> Get()
        {
            List<SubjectEntity> subjects = await _subjectRepository
                .GetAsync()
                .ToListAsync();

            return subjects.SelectToList(entity => new SubjectProfileDto(entity));

        }

        public async Task<SubjectProfileDto> Get(int id)
        {
            SubjectEntity subject = await _subjectRepository
                .GetAsync()
                .FirstAsync(s => s.Id == id);

            return new SubjectProfileDto(subject);
        }

        public async Task<List<SubjectProfileDto>> GetGroupSubjects(int groupId)
        {
            List<SubjectEntity> subjectEntities = await _groupSubjectRepository
                .GetAsync()
                .SearchSubjects(StudySearchParametersDto.ForGroup(groupId))
                .ToListAsync();

            return subjectEntities.SelectToList(entity => new SubjectProfileDto(entity));
        }

        public async Task<List<SubjectProfileDto>> GetSubjectsForDtoAsync(StudySearchParametersDto searchParametersDto)
        {
            List<SubjectEntity> subjectEntities = await _groupSubjectRepository
                .GetAsync()
                .SearchSubjects(searchParametersDto)
                .ToListAsync();
            return subjectEntities.SelectToList(entity => new SubjectProfileDto(entity));
        }
    }
}