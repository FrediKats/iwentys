using System.Collections.Generic;

namespace Iwentys.Features.Study.Entities
{
    public class Subject
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public virtual ICollection<GroupSubject> GroupSubjects { get; set; }
    }
}