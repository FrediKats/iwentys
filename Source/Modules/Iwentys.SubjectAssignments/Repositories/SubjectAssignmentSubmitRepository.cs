using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.SubjectAssignments
{
    public static class SubjectAssignmentSubmitRepository
    {
        //TODO: move to domain as Expression
        //TODO: filter with all parameters
        public static IQueryable<SubjectAssignmentSubmit> ApplySearch(IQueryable<SubjectAssignmentSubmit> query, SubjectAssignmentSubmitSearchArguments searchArguments)
        {
            return query
                .Where(sas => sas.SubjectAssignment.SubjectId == searchArguments.SubjectId)
                .WhereIf(searchArguments.StudentId, sas => sas.StudentId == searchArguments.StudentId);
        }
    }
}