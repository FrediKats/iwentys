using System.Collections.Generic;
using Iwentys.Database.Seeding.EntityGenerators;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;

namespace Iwentys.Database.Seeding
{
    public class DatabaseContextGenerator
    {
        public const int TeacherCount = 20;
        public DatabaseContextGenerator()
        {
            InitStudyTables();
        }

        public List<TeacherEntity> Teachers { get; set; }
        public List<SubjectEntity> Subjects { get; set; }
        public List<StudyGroupEntity> StudyGroups { get; set; }
        public List<StudyCourseEntity> StudyCourses { get; set; }
        public List<StudyProgramEntity> StudyPrograms { get; set; }
        public List<GroupSubjectEntity> GroupSubjects { get; set; }

        public List<StudentEntity> Students { get; set; }
        public List<GuildEntity> Guilds { get; set; }
        public List<GuildMemberEntity> GuildMembers { get; set; }
        public List<GuildPinnedProjectEntity> GuildPinnedProjects { get; set; }

        public List<GuildAchievementEntity> GuildAchievementModels { get; set; }
        public List<StudentAchievementEntity> StudentAchievementModels { get; set; }

        private void InitStudyTables()
        {
            Teachers = new TeacherGenerator().Faker.Generate(TeacherCount);
            StudyPrograms = new List<StudyProgramEntity> { new StudyProgramEntity { Id = 1, Name = "ИС" } };

            StudyCourses = new List<StudyCourseEntity>
            {
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y20),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y21),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y22),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y23),
                DatabaseContextSetup.Create.IsCourse(StudentGraduationYear.Y24)
            };
        }
    }
}