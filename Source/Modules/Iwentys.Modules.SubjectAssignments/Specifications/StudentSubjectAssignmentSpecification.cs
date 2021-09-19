using System.Linq;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Enums;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Modules.SubjectAssignments.Specifications
{
    public class StudentSubjectAssignmentSpecification : ISpecification<GroupSubjectAssignment, SubjectAssignment>
    {
        private readonly Student _student;

        public StudentSubjectAssignmentSpecification(Student student)
        {
            _student = student;
        }

        public IQueryable<SubjectAssignment> Specify(IQueryable<GroupSubjectAssignment> queryable)
        {
            return queryable
                .Where(gsa => gsa.GroupId == _student.GroupId)
                .Select(gsa => gsa.SubjectAssignment)
                .Where(sa => sa.AvailabilityState == AvailabilityState.Visible);
        }
    }
}
