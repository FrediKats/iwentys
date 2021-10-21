﻿using System.Linq;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Enums;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Modules.SubjectAssignments.Specifications
{
    public class StudentSubjectAssignmentSpecification : ISpecification<GroupSubjectAssignment, SubjectAssignment>
    {
        private readonly Student _student;
        private readonly int _subjectId;

        public StudentSubjectAssignmentSpecification(Student student, int subjectId)
        {
            _student = student;
            _subjectId = subjectId;
        }

        public IQueryable<SubjectAssignment> Specify(IQueryable<GroupSubjectAssignment> queryable)
        {
            return queryable
                .Where(gsa => gsa.GroupId == _student.GroupId)
                .Select(gsa => gsa.SubjectAssignment)
                .Where(gsa => gsa.Subject.Id == _subjectId)
                .Where(sa => sa.AvailabilityState == AvailabilityState.Visible);
        }
    }
}