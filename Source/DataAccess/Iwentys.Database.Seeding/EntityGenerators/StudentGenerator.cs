using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Common.Tools;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class StudentGenerator : IEntityGenerator
    {
        private const int StudentCount = 200;

        public StudentGenerator(List<StudyGroup> studyGroups)
        {
            StudyGroupMembers = new List<StudyGroupMember>();
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

            foreach (Student student in Students)
            {
                StudyGroupMembers.Add(new StudyGroupMember {StudentId = student.Id, GroupId = RandomExtensions.Instance.PickRandom(studyGroups).Id });
            }

            StudyGroupMembers
                .GroupBy(s => s.GroupId)
                .Select(g => g.FirstOrDefault())
                .Where(s => s is not null)
                .ToList()
                .ForEach(s =>
                {
                    StudyGroup studyGroup = studyGroups.First();
                    studyGroup.GroupAdminId = s.StudentId;
                });

            StudyGroupMember studyGroupMember = StudyGroupMembers.First(sgm => sgm.StudentId == 228617);
            studyGroupMember.GroupId = studyGroups.First(g => g.GroupName.Contains("3505")).Id;
        }

        public List<Student> Students { get; set; }
        public List<StudyGroupMember> StudyGroupMembers { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(Students);
            modelBuilder.Entity<StudyGroupMember>().HasData(StudyGroupMembers);
        }
    }
}