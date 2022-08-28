using System.ComponentModel.DataAnnotations;

namespace Iwentys.Domain.Study;

public class StudentPosition
{
    [Key]
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    public int CourseId { get; set; }
}