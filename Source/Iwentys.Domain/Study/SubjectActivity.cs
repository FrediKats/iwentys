namespace Iwentys.Domain.Study;

public class SubjectActivity
{
    public int SubjectId { get; init; }
    public StudentPosition StudentPosition { get; set; }

    public int StudentId { get; init; }
    public double Points { get; set; }
}