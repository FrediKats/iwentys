﻿using System.Collections.Generic;

namespace Iwentys.Domain.AccountManagement.Mentors.Dto
{
    public class SubjectMentorCreateArgs
    {
        public int SubjectId { get; set; }
        public int MentorId { get; set; }
        public List<int> GroupSubjectIds { get; set; }
    }
}