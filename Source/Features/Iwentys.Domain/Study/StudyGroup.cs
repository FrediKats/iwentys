using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; init; }

        public int StudyCourseId { get; init; }
        public virtual StudyCourse StudyCourse { get; set; }

        public int? GroupAdminId { get; set; }
        //public Student GroupAdmin { get; set; }

        public virtual List<Student> Students { get; set; }
        public virtual List<GroupSubject> GroupSubjects { get; set; }

        public static Expression<Func<StudyGroup, bool>> IsMatch(GroupName groupName)
        {
            return studyGroup => studyGroup.GroupName == groupName.Name;
        }

        public StudyGroup()
        {
            Students = new List<Student>();
            GroupSubjects = new List<GroupSubject>();
        }

        public static StudyGroup MakeGroupAdmin(IwentysUser initiatorProfile, Student newGroupAdmin)
        {
            SystemAdminUser admin = initiatorProfile.EnsureIsAdmin();
            if (newGroupAdmin.Group is null)
            {
                //TODO: add exception
            }
            else
            {
                newGroupAdmin.Group.GroupAdminId = newGroupAdmin.Id;
            }

            return newGroupAdmin.Group;
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }
    }
}