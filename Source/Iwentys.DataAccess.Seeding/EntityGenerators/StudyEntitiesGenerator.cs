﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding;

public class StudyEntitiesGenerator : IEntityGenerator
{
    //TODO: do it good
    private const int MentorId = 228617;

    private const int TeacherCount = 20;
    private const int SubjectCount = 8;
    public StudyEntitiesGenerator()
    {
        Teachers = UsersFaker.Instance.UniversitySystemUsers
            .Generate(TeacherCount)
            .SelectToList(UniversitySystemUser.Create);
        Teachers.ForEach(t => t.Id = UsersFaker.Instance.GetIdentifier());

        Subjects = SubjectFaker.Instance.Generate(SubjectCount);

        StudyGroups = ReadGroups();
        GroupSubjects = new List<GroupSubject>();
        foreach (Subject subject in Subjects)
        foreach (StudyGroup studyGroup in StudyGroups)
        {
            GroupSubjects.Add(CreateGroupSubjectEntity(studyGroup, subject));
        }
    }

    public List<Subject> Subjects { get; set; }
    public List<GroupSubject> GroupSubjects { get; set; }
    public List<StudyGroup> StudyGroups { get; set; }
    public List<UniversitySystemUser> Teachers { get; set; }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyGroup>().HasData(StudyGroups);
        modelBuilder.Entity<UniversitySystemUser>().HasData(Teachers);
        modelBuilder.Entity<Subject>().HasData(Subjects);
        modelBuilder.Entity<GroupSubject>().HasData(GroupSubjects);
    }

    private GroupSubject CreateGroupSubjectEntity(StudyGroup group, Subject subject)
    {
        //FYI: we do not init SerializedGoogleTableConfig here
        return new GroupSubject
        {
            Id = Create.GroupSubjectIdentifierGenerator.Next(),
            SubjectId = subject.Id,
            StudyGroupId = group.Id,
        };
    }

    private static List<StudyGroup> ReadGroups()
    {
        var result = new List<StudyGroup>();
        result.AddRange(CourseGroup(1, 5, 3, 9));
        result.AddRange(CourseGroup(2, 4, 2, 10));
        result.AddRange(CourseGroup(3, 3, 1, 9));
        result.AddRange(CourseGroup(4, 2, 1, 12));
        result.AddRange(CourseGroup(5, 1, 1, 12));

        for (var i = 0; i < result.Count; i++)
            result[i].Id = i + 1;

        return result;
    }

    public static List<StudyGroup> CourseGroup(int courseId, int course, int firstGroup, int lastGroup)
    {
        return Enumerable
            .Range(firstGroup, lastGroup - firstGroup + 1)
            .Select(g => new StudyGroup
            {
                CourseId = courseId,
                GroupName = $"M3{course}{g:00}"
            })
            .ToList();
    }

    public static class Create
    {
        private static readonly IdentifierGenerator CourseIdentifierGenerator = new IdentifierGenerator();
        public static readonly IdentifierGenerator GroupSubjectIdentifierGenerator = new IdentifierGenerator();
    }
}