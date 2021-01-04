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
        
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<GroupSubject> _groupSubjectRepository;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _subjectRepository = _unitOfWork.GetRepository<Subject>();
            _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubject>();
        }

        public async Task<List<SubjectProfileDto>> Get()
        {
            List<Subject> subjects = await _subjectRepository
                .Get()
                .ToListAsync();

            return subjects.SelectToList(entity => new SubjectProfileDto(entity));

        }

        public async Task<SubjectProfileDto> Get(int id)
        {
            Subject subject = await _subjectRepository
                .Get()
                .FirstAsync(s => s.Id == id);

            return new SubjectProfileDto(subject);
        }

        public async Task<List<SubjectProfileDto>> GetGroupSubjects(int groupId)
        {
            List<Subject> subjectEntities = await _groupSubjectRepository
                .Get()
                .SearchSubjects(StudySearchParametersDto.ForGroup(groupId))
                .ToListAsync();

            return subjectEntities.SelectToList(entity => new SubjectProfileDto(entity));
        }

        public async Task<List<SubjectProfileDto>> GetSubjectsForDto(StudySearchParametersDto searchParametersDto)
        {
            List<Subject> subjectEntities = await _groupSubjectRepository
                .Get()
                .SearchSubjects(searchParametersDto)
                .ToListAsync();
            return subjectEntities.SelectToList(entity => new SubjectProfileDto(entity));
        }
    }
}