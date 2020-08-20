using Iwentys.Models.Entities.Study;

namespace Iwentys.Models.Transferable.Students
{
    public class SubjectActivityInfoDto
    {
        public string SubjectTitle { get; set; }
        public double Points { get; set; }

        public SubjectActivityInfoDto(SubjectActivity subjectActivity) : this()
        {
            SubjectTitle = subjectActivity.SubjectForGroup.Subject.Name;
            Points = subjectActivity.Points;
        }

        private SubjectActivityInfoDto()
        {
        }
    }
}