using Iwentys.EntityManager.Domain.Accounts;

namespace Iwentys.EntityManager.Domain;

public class GroupSubjectMentor
{
    public bool IsLector { get; set; }
        
    public int UserId { get; set; }
    public virtual IwentysUser User { get; set; }

    public int GroupSubjectId { get; set; }
    public virtual GroupSubject GroupSubject { get; set; }
}