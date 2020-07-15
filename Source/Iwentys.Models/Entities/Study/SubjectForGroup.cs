using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Study
{
    public class SubjectForGroup
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int StudyGroupId { get; set; }
        public StudyGroup StudyGroup { get; set; }

        public int LecturerId { get; set; }
        public Teacher Lecturer { get; set; }


        public StudySemester StudySemester { get; set; }
    }
}