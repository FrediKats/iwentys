using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.StudentFeature.ViewModels
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