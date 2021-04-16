using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Iwentys.Domain.Study
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; init; }

        public int StudyCourseId { get; init; }
        public virtual StudyCourse StudyCourse { get; set; }

        //FYI: looks like hack
        public int? GroupAdminId { get; set; }

        public virtual List<Student> Students { get; set; }
        public virtual List<GroupSubject> GroupSubjects { get; set; }

        public static Expression<Func<StudyGroup, bool>> IsMatch(GroupName groupName)
        {
            return studyGroup => studyGroup.GroupName == groupName.Name;
        }
    }
}