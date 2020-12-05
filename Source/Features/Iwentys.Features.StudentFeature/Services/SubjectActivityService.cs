using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.StudentFeature.Services
{
    public class SubjectActivityService
    {
        private readonly ISubjectActivityRepository _subjectActivityRepository;

        public SubjectActivityService(ISubjectActivityRepository subjectActivityRepository)
        {
            _subjectActivityRepository = subjectActivityRepository;
        }

        public Task<List<SubjectActivityEntity>> GetStudentActivity(int studentId)
        {
            return _subjectActivityRepository
                .Read()
                .Where(s => s.StudentId == studentId)
                .ToListAsync();
        }
    }
}