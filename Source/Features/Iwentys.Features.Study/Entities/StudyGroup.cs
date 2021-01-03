using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Features.Study.Domain;

namespace Iwentys.Features.Study.Entities
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; init; }

        public int StudyCourseId { get; init; }
        public virtual StudyCourse StudyCourse { get; init; }

        //FYI: looks like hack
        public int? GroupAdminId { get; set; }

        public virtual List<StudyGroupMember> Students { get; init; }
        public virtual List<GroupSubject> GroupSubjects { get; init; }

        public static Expression<Func<StudyGroup, bool>> IsMatch(GroupName groupName) => studyGroup => studyGroup.GroupName == groupName.Name;
    }
}