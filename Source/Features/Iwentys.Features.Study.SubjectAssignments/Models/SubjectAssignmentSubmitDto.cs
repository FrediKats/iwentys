using System;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.SubjectAssignments.Entities;

namespace Iwentys.Features.Study.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitDto
    {
        public StudentInfoDto Student { get; set; }

        public DateTime SubmitTimeUtc { get; set; }

        public SubjectAssignmentSubmitDto(SubjectAssignmentSubmit submit)
        {
            Student = new StudentInfoDto(submit.Student);
            SubmitTimeUtc = submit.SubmitTimeUtc;
        }
    }
}