using System;
using System.Collections.Generic;
using System.Text;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Study
{
    public class StudySearchDto
    {
        public int? GroupId { get; set; }
        public int? SubjectId { get; set; }
        public int? StreamId { get; set; }
        public StudySemester? StudySemester { get; set; }
    }
}
