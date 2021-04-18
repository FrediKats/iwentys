using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class SubjectService
    {
        private readonly IGenericRepository<GroupSubject> _groupSubjectRepository;

        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _subjectRepository = _unitOfWork.GetRepository<Subject>();
            _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubject>();
        }

        public async Task<List<SubjectProfileDto>> Get()
        {
            return await _subjectRepository
                .Get()
                .Select(entity => new SubjectProfileDto(entity))
                .ToListAsync();
        }

        public async Task<SubjectProfileDto> Get(int id)
        {
            return await _subjectRepository
                .Get()
                .FirstAsync(s => s.Id == id)
                .To(entity => new SubjectProfileDto(entity));
        }

        public async Task<List<SubjectProfileDto>> GetGroupSubjects(int groupId)
        {
            return await _groupSubjectRepository
                .Get()
                .SearchSubjects(StudySearchParametersDto.ForGroup(groupId))
                .Select(entity => new SubjectProfileDto(entity))
                .ToListAsync();
        }

        public async Task<List<SubjectProfileDto>> GetSubjectsForDto(StudySearchParametersDto searchParametersDto)
        {
            return await _groupSubjectRepository
                .Get()
                .SearchSubjects(searchParametersDto)
                .Select(entity => new SubjectProfileDto(entity))
                .ToListAsync();
        }
    }
}