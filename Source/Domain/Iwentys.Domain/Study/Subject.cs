using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study
{
    public class Subject
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public virtual ICollection<GroupSubject> GroupSubjects { get; set; }

        public Subject()
        {
            GroupSubjects = new List<GroupSubject>();
        }

        public bool HasMentorPermission(IwentysUser user)
        {
            return GroupSubjects.Any(gs => gs.LectorTeacherId == user.Id || gs.PracticeTeacherId == user.Id);
        }
    }
}