using System.Linq;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Infrastructure.Application.Repositories
{
    public class SubjectAssignmentSubmitRepository
    {
        public static IQueryable<SubjectAssignmentSubmit> ApplySearch(IQueryable<SubjectAssignmentSubmit> query, SubjectAssignmentSubmitSearchArguments searchArguments)
        {
            return query
                .Where(sas => sas.SubjectAssignment.SubjectId == searchArguments.SubjectId)
                .WhereIf(searchArguments.StudentId, sas => sas.StudentId == searchArguments.StudentId);
        }
    }
}