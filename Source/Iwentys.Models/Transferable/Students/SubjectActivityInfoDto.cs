using Iwentys.Models.Entities.Study;

namespace Iwentys.Models.Transferable.Students
{
    public class SubjectActivityInfoDto
    {
        public SubjectActivityInfoDto(SubjectActivityEntity subjectActivity) : this()
        {
            SubjectTitle = subjectActivity.GroupSubjectEntity.Subject.Name;
            Points = subjectActivity.Points;
        }

        public SubjectActivityInfoDto()
        {
        }

        public string SubjectTitle { get; set; }
        public double Points { get; set; }
    }
}