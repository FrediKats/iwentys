using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Common;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding;

public class StudentGenerator : IEntityGenerator
{
    private const int StudentCount = 200;

    public StudentGenerator(List<StudyGroup> studyGroups)
    {
        Students = UsersFaker.Instance.Students
            .Generate(StudentCount)
            .SelectToList(Student.Create);

        Students.ForEach(s => s.Id = UsersFaker.Instance.GetIdentifier());

        Students.Add(new Student
        {
            Id = 228617,
            FirstName = "Фреди",
            MiddleName = "Кисикович",
            SecondName = "Катс",
            IsAdmin = true,
            GithubUsername = "InRedikaWB",
            CreationTime = DateTime.UtcNow,
            LastOnlineTime = DateTime.UtcNow,
            BarsPoints = short.MaxValue,
            AvatarUrl = new Faker().Image.PicsumUrl()
        });

        foreach (Student student in Students) student.GroupId = RandomExtensions.Instance.PickRandom(studyGroups).Id;

        Students
            .GroupBy(s => s.GroupId)
            .Select(g => g.FirstOrDefault())
            .Where(s => s is not null)
            .ToList()
            .ForEach(s =>
            {
                StudyGroup studyGroup = studyGroups.First();
                studyGroup.GroupAdminId = s.Id;
            });

        Students.First(sgm => sgm.Id == 228617).GroupId = studyGroups.First(g => g.GroupName.Contains("3505")).Id;
    }

    public List<Student> Students { get; set; }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(Students);
    }
}