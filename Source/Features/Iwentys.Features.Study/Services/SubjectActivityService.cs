using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    //TODO: move this service to ??
    public class SubjectActivityService
    {
        private readonly IGenericRepository<SubjectActivity> _subjectActivityRepositoryNew;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectActivityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _subjectActivityRepositoryNew = _unitOfWork.GetRepository<SubjectActivity>();
        }

        public Task<List<SubjectActivity>> GetStudentActivity(int studentId)
        {
            return _subjectActivityRepositoryNew
                .Get()
                .Where(s => s.StudentId == studentId)
                .ToListAsync();
        }
    }
}