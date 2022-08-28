using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Study;

public class StudyGroup
{
    public int Id { get; set; }
    public string GroupName { get; init; }

    public int? GroupAdminId { get; set; }
    //public Student GroupAdmin { get; set; }

    public int CourseId { get; set; }

    public virtual List<Student> Students { get; set; }

    public StudyGroup()
    {
        Students = new List<Student>();
    }

    public void AddStudent(Student student)
    {
        Students.Add(student);
    }

    public void MakeAdmin(IwentysUser initiatorProfile, Student newGroupAdmin)
    {
        SystemAdminUser admin = initiatorProfile.EnsureIsAdmin();
        GroupAdminId = newGroupAdmin.Id;
    }
}