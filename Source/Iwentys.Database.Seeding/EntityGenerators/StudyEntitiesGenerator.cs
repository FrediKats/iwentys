using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class StudyEntitiesGenerator
    {
        private const int TeacherCount = 20;
        private const int SubjectCount = 8;
        private const StudySemester CurrentSemester = StudySemester.Y20H1;

        public List<StudyCourseEntity> StudyCourses { get; set; }
        public List<StudyProgramEntity> StudyPrograms { get; set; }
        public List<SubjectEntity> Subjects { get; set; }
        public List<GroupSubjectEntity> GroupSubjects { get; set; }
        public List<StudyGroupEntity> StudyGroups { get; set; }
        public List<TeacherEntity> Teachers { get; set; }

        public StudyEntitiesGenerator()
        {
            Teachers = new TeacherGenerator().Faker.Generate(TeacherCount);
            Subjects = new SubjectGenerator().Faker.Generate(SubjectCount);
            StudyPrograms = new List<StudyProgramEntity> { new StudyProgramEntity { Id = 1, Name = "ИС" } };
            StudyCourses = new List<StudyCourseEntity>
            {
                Create.IsCourse(StudentGraduationYear.Y20),
                Create.IsCourse(StudentGraduationYear.Y21),
                Create.IsCourse(StudentGraduationYear.Y22),
                Create.IsCourse(StudentGraduationYear.Y23),
                Create.IsCourse(StudentGraduationYear.Y24)
            };

            StudyGroups = ReadGroups();
            GroupSubjects = new List<GroupSubjectEntity>();

            foreach (SubjectEntity subject in Subjects)
            foreach (StudyGroupEntity studyGroup in StudyGroups)
                GroupSubjects.Add(CreateGroupSubjectEntity(studyGroup, subject));

        }

        private GroupSubjectEntity CreateGroupSubjectEntity(StudyGroupEntity groupEntity, SubjectEntity subject)
        {
            //FYI: we do not init SerializedGoogleTableConfig here
            return new GroupSubjectEntity
            {
                Id = Create.GroupSubjectIdentifierGenerator.Next(),
                SubjectId = subject.Id,
                StudyGroupId = groupEntity.Id,
                LectorTeacherId = Teachers.GetRandom().Id,
                PracticeTeacherId = Teachers.GetRandom().Id,
                StudySemester = CurrentSemester
            };
        }

        private static List<StudyGroupEntity> ReadGroups()
        {
            var result = new List<StudyGroupEntity>();
            result.AddRange(CourseGroup(1, 5, 3, 9));
            result.AddRange(CourseGroup(2, 4, 2, 10));
            result.AddRange(CourseGroup(3, 3, 1, 9));
            result.AddRange(CourseGroup(4, 2, 1, 12));
            result.AddRange(CourseGroup(5, 1, 1, 12));

            for (var i = 0; i < result.Count; i++)
                result[i].Id = i + 1;

            return result;
        }

        public static List<StudyGroupEntity> CourseGroup(int courseId, int course, int firstGroup, int lastGroup)
        {
            return Enumerable
                .Range(firstGroup, lastGroup - firstGroup + 1)
                .Select(g => new StudyGroupEntity
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

            public static StudyCourseEntity IsCourse(StudentGraduationYear year)
            {
                return new StudyCourseEntity
                {
                    Id = CourseIdentifierGenerator.Next(),
                    GraduationYear = year,
                    StudyProgramId = 1
                };
            }
        }
    }
}