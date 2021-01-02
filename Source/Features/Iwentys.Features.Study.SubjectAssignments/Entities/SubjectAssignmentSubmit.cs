using System;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.SubjectAssignments.Entities
{
    public class SubjectAssignmentSubmit
    {
        public int Id { get; set; }

        public int SubjectAssignmentId { get; set; }
        public virtual SubjectAssignment SubjectAssignment { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public DateTime SubmitTimeUtc { get; set; }

        //TODO: add info about submit: comments, link to project
    }
}