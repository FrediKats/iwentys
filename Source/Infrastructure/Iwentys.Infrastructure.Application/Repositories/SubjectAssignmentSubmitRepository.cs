using System.Linq;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Infrastructure.Application.Repositories
{
    public static class SubjectAssignmentSubmitRepository
    {
        //TODO: move to domain as Expression
        public static IQueryable<SubjectAssignmentSubmit> ApplySearch(IQueryable<SubjectAssignmentSubmit> query, SubjectAssignmentSubmitSearchArguments searchArguments)
        {
            return query
                .Where(sas => sas.SubjectAssignment.SubjectId == searchArguments.SubjectId)
                .WhereIf(searchArguments.StudentId, sas => sas.StudentId == searchArguments.StudentId);
        }
    }
}