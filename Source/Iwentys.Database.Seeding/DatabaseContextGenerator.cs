using Iwentys.Database.Seeding.EntityGenerators;

namespace Iwentys.Database.Seeding
{
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            StudyEntitiesGenerator = new StudyEntitiesGenerator();
            StudentGenerator = new StudentGenerator(StudyEntitiesGenerator.StudyGroups);
            GithubDataGenerator = new GithubDataGenerator(StudentGenerator.Students);
            GuildGenerator = new GuildGenerator(StudentGenerator.Students, GithubDataGenerator.GithubProjectEntities);
            AchievementGenerator = new AchievementGenerator(StudentGenerator.Students, GuildGenerator.Guilds);
            SubjectActivityGenerator = new SubjectActivityGenerator(StudyEntitiesGenerator.GroupSubjects, StudentGenerator.Students);
            AssignmentGenerator = new AssignmentGenerator(StudentGenerator.Students);
            NewsfeedGenerator = new NewsfeedGenerator(StudentGenerator.Students, GuildGenerator.Guilds, StudyEntitiesGenerator.Subjects);
            QuestGenerator = new QuestGenerator(StudentGenerator.Students);
        }

        public StudyEntitiesGenerator StudyEntitiesGenerator { get; set; }
        public StudentGenerator StudentGenerator { get; set; }
        public GuildGenerator GuildGenerator { get; set; }
        public AchievementGenerator AchievementGenerator { get; set; }
        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
        public AssignmentGenerator AssignmentGenerator { get; set; }
        public GithubDataGenerator GithubDataGenerator { get; set; }
        public NewsfeedGenerator NewsfeedGenerator { get; set; }
        public QuestGenerator QuestGenerator { get; set; }
    }
}