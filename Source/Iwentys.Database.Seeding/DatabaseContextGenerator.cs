using Iwentys.Database.Seeding.EntityGenerators;

namespace Iwentys.Database.Seeding
{
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            SubjectActivityGenerator = new SubjectActivityGenerator();
            StudentGenerator = new StudentGenerator(SubjectActivityGenerator.StudyGroups);
            GuildGenerator = new GuildGenerator(StudentGenerator.Students);
        }

        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
        public StudentGenerator StudentGenerator { get; set; }
        public GuildGenerator GuildGenerator { get; set; }
    }
}