using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record SubjectActivityInfoResponseDto
    {
        public SubjectActivityInfoResponseDto(SubjectActivityEntity subjectActivity)
            : this(subjectActivity.GroupSubject.Subject.Name, subjectActivity.Points)
        {
        }

        public SubjectActivityInfoResponseDto(string subjectTitle, double points) : this()
        {
            SubjectTitle = subjectTitle;
            Points = points;
        }

        public SubjectActivityInfoResponseDto()
        {
        }
        
        public string SubjectTitle { get; set; }
        public double Points { get; set; }
    }
}