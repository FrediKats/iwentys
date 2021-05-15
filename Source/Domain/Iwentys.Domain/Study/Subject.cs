using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Enums;

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

        public GroupSubject AddGroup(StudyGroup studyGroup, StudySemester studySemester, UniversitySystemUser lector = null, UniversitySystemUser practice = null)
        {
            var groupSubject = new GroupSubject(this, studyGroup, studySemester, lector, practice);
            GroupSubjects.Add(groupSubject);
            return groupSubject;
        }

        public bool HasMentorPermission(IwentysUser user)
        {
            return GroupSubjects.Any(gs => gs.LectorMentorId == user.Id || gs.PracticeMentorId == user.Id);
        }
    }
}