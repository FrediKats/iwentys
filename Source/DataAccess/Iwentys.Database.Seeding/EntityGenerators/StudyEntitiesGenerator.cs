using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Database.Seeding.FakerEntities.Study;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class StudyEntitiesGenerator : IEntityGenerator
    {
        private const int TeacherCount = 20;
        private const int SubjectCount = 8;
        private const StudySemester CurrentSemester = StudySemester.Y20H1;

        public StudyEntitiesGenerator()
        {
            Teachers = UsersFaker.Instance.UniversitySystemUsers
                .Generate(TeacherCount)
                .SelectToList(UniversitySystemUser.Create);
            Teachers.ForEach(t => t.Id = UsersFaker.Instance.GetIdentifier());

            Subjects = SubjectFaker.Instance.Generate(SubjectCount);
            StudyPrograms = new List<StudyProgram> {new StudyProgram {Id = 1, Name = "ИС"}};
            StudyCourses = new List<StudyCourse>
            {
                Create.IsCourse(StudentGraduationYear.Y20),
                Create.IsCourse(StudentGraduationYear.Y21),
                Create.IsCourse(StudentGraduationYear.Y22),
                Create.IsCourse(StudentGraduationYear.Y23),
                Create.IsCourse(StudentGraduationYear.Y24)
            };

            StudyGroups = ReadGroups();
            GroupSubjects = new List<GroupSubject>();

            foreach (Subject subject in Subjects)
            foreach (StudyGroup studyGroup in StudyGroups)
                GroupSubjects.Add(CreateGroupSubjectEntity(studyGroup, subject));
        }

        public List<StudyCourse> StudyCourses { get; set; }
        public List<StudyProgram> StudyPrograms { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<GroupSubject> GroupSubjects { get; set; }
        public List<StudyGroup> StudyGroups { get; set; }
        public List<UniversitySystemUser> Teachers { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudyProgram>().HasData(StudyPrograms);
            modelBuilder.Entity<StudyCourse>().HasData(StudyCourses);
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
                LectorTeacherId = RandomExtensions.Instance.PickRandom(Teachers).Id,
                PracticeTeacherId = RandomExtensions.Instance.PickRandom(Teachers).Id,
                StudySemester = CurrentSemester
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
                    StudyCourseId = courseId,
                    GroupName = $"M3{course}{g:00}"
                })
                .ToList();
        }

        public static class Create
        {
            private static readonly IdentifierGenerator CourseIdentifierGenerator = new IdentifierGenerator();
            public static readonly IdentifierGenerator GroupSubjectIdentifierGenerator = new IdentifierGenerator();

            public static StudyCourse IsCourse(StudentGraduationYear year)
            {
                return new StudyCourse
                {
                    Id = CourseIdentifierGenerator.Next(),
                    GraduationYear = year,
                    StudyProgramId = 1
                };
            }
        }
    }
}