using System.Collections.Generic;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Study
{
    public class StudyStream
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StudyGroup> Groups { get; set; }
        public StudySemester StudySemester { get; set; }
    }
}
