using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study
{
    public class GroupSubjectMentor
    {
        public bool IsLector { get; set; }
        
        public int UserId { get; set; }
        public virtual IwentysUser User { get; set; }

        public int GroupSubjectId { get; set; }
        public virtual GroupSubject GroupSubject { get; set; }
    }
}