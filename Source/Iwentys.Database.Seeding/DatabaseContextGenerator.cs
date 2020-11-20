using Iwentys.Database.Seeding.EntityGenerators;

namespace Iwentys.Database.Seeding
{
    //TODO: Remove after release
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            SubjectActivityGenerator = new SubjectActivityGenerator();
            StudentGenerator = new StudentGenerator(SubjectActivityGenerator.StudyGroups);
            GuildGenerator = new GuildGenerator(StudentGenerator.Students);
            AchievementGenerator = new AchievementGenerator(StudentGenerator.Students, GuildGenerator.Guilds);
        }

        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
        public StudentGenerator StudentGenerator { get; set; }
        public GuildGenerator GuildGenerator { get; set; }
        public AchievementGenerator AchievementGenerator { get; set; }
    }
}