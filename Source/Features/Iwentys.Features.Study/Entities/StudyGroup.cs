using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Domain;

namespace Iwentys.Features.Study.Entities
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; init; }

        public int StudyCourseId { get; init; }
        public virtual StudyCourse StudyCourse { get; init; }

        //FYI: it's not join Student table :c
        public virtual List<Student> Students { get; init; }
        public virtual List<GroupSubject> GroupSubjects { get; init; }

        public static Expression<Func<StudyGroup, bool>> IsMatch(GroupName groupName) => studyGroup => studyGroup.GroupName == groupName.Name;
    }
}