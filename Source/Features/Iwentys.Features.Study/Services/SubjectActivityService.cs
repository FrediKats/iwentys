using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class SubjectActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<SubjectActivityEntity> _subjectActivityRepositoryNew;

        public SubjectActivityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _subjectActivityRepositoryNew = _unitOfWork.GetRepository<SubjectActivityEntity>();
        }

        public Task<List<SubjectActivityEntity>> GetStudentActivity(int studentId)
        {
            return _subjectActivityRepositoryNew
                .Get()
                .Where(s => s.StudentId == studentId)
                .ToListAsync();
        }
    }
}