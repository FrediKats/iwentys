using Iwentys.Domain.Study;

namespace Iwentys.Study;

public record SubjectActivityInfoResponseDto
{
    public SubjectActivityInfoResponseDto(SubjectActivity subjectActivity)
        : this(subjectActivity.GroupSubject.Subject.Title, subjectActivity.Points)
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