using System.Collections.Generic;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectActivityGenerator
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
                Id = DatabaseContextSetup.Create.GroupSubjectIdentifierGenerator.Next(),
                SubjectId = subject.Id,
                StudyGroupId = groupEntity.Id,
                LectorTeacherId = Teachers.GetRandom().Id,
                PracticeTeacherId = Teachers.GetRandom().Id,
                StudySemester = CurrentSemester
            };
        }
    }
}