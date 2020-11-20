using System.Collections.Generic;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectActivityGenerator
    {
        public const int TeacherCount = 20;
        public const int SubjectCount = 8;

        public List<StudyCourseEntity> StudyCourses { get; set; }
        public List<StudyProgramEntity> StudyPrograms { get; set; }
        public List<SubjectEntity> Subjects { get; set; }
        public List<GroupSubjectEntity> GroupSubjects { get; set; }
        public List<StudyGroupEntity> StudyGroups { get; set; }
        public List<TeacherEntity> Teachers { get; set; }

        public SubjectActivityGenerator()
        {
            Teachers = new TeacherGenerator().Faker.Generate(TeacherCount);
            Subjects = new SubjectGenerator().Faker.Generate(SubjectCount);
            StudyPrograms = new List<StudyProgramEntity> { new StudyProgramEntity { Id = 1, Name = "ИС" } };
            StudyCourses = new List<StudyCourseEntity>
            {
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y20),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y21),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y22),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y23),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y24)
            };

            StudyGroups = new StudentMockDataReader().ReadGroups();
        }
    }
}