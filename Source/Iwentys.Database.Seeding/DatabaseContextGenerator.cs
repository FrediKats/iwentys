using Iwentys.Database.Seeding.EntityGenerators;

namespace Iwentys.Database.Seeding
{
    //TODO: Remove after release
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            StudyEntitiesGenerator = new StudyEntitiesGenerator();
            StudentGenerator = new StudentGenerator(StudyEntitiesGenerator.StudyGroups);
            GuildGenerator = new GuildGenerator(StudentGenerator.Students);
            AchievementGenerator = new AchievementGenerator(StudentGenerator.Students, GuildGenerator.Guilds);
            SubjectActivityGenerator = new SubjectActivityGenerator(StudyEntitiesGenerator.GroupSubjects, StudentGenerator.Students);
            AssignmentGenerator = new AssignmentGenerator(StudentGenerator.Students);
            GithubActivityGenerator = new GithubActivityGenerator(StudentGenerator.Students);
            NewsfeedGenerator = new NewsfeedGenerator(StudentGenerator.Students, GuildGenerator.Guilds, StudyEntitiesGenerator.Subjects);
        }

        public StudyEntitiesGenerator StudyEntitiesGenerator { get; set; }
        public StudentGenerator StudentGenerator { get; set; }
        public GuildGenerator GuildGenerator { get; set; }
        public AchievementGenerator AchievementGenerator { get; set; }
        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
        public AssignmentGenerator AssignmentGenerator { get; set; }
        public GithubActivityGenerator GithubActivityGenerator { get; set; }
        public NewsfeedGenerator NewsfeedGenerator { get; set; }
    }
}