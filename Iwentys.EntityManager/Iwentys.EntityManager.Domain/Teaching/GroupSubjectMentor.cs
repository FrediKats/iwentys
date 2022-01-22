namespace Iwentys.EntityManager.Domain;

public class GroupSubjectTeacher
{
    public int TeacherId { get; set; }
    public virtual IwentysUser Teacher { get; set; }

    public int GroupSubjectId { get; set; }
    public virtual GroupSubject GroupSubject { get; set; }

    public TeacherType TeacherType { get; set; }
}