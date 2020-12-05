using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record SubjectActivityInfoResponseDto(string SubjectTitle, double Points)
    {
        public SubjectActivityInfoResponseDto(SubjectActivityEntity subjectActivity) : this(subjectActivity.GroupSubject.Subject.Name, subjectActivity.Points)
        {
        }
    }
}