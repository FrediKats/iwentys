using Iwentys.Models.Entities.Study;

namespace Iwentys.Models.Transferable.Students
{
    public class SubjectActivityInfoResponse
    {
        public SubjectActivityInfoResponse(SubjectActivityEntity subjectActivity) : this()
        {
            SubjectTitle = subjectActivity.GroupSubject.Subject.Name;
            Points = subjectActivity.Points;
        }

        public SubjectActivityInfoResponse()
        {
        }

        public string SubjectTitle { get; set; }
        public double Points { get; set; }
    }
}