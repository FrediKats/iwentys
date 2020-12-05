using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
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