﻿using System;
using Iwentys.Features.Assignments.Models;

namespace Iwentys.Features.Study.SubjectAssignments.Models
{
    public class SubjectAssignmentCreateArguments
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime DeadlineUtc { get; set; }

        public AssignmentCreateArguments ConvertToAssignmentCreateArguments(int subjectId)
        {
            return new AssignmentCreateArguments
            {
                Title = Title,
                Description = Description,
                DeadlineTimeUtc = DeadlineUtc,
                SubjectId = subjectId,
                Link = Link
            };
        }
    }
}