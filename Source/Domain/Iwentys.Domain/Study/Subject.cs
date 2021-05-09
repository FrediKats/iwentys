using System.Collections.Generic;

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
    }
}