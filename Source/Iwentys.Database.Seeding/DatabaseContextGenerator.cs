using Iwentys.Database.Seeding.EntityGenerators;

namespace Iwentys.Database.Seeding
{
    public class DatabaseContextGenerator
    {
        public DatabaseContextGenerator()
        {
            SubjectActivityGenerator = new SubjectActivityGenerator();
        }

        public SubjectActivityGenerator SubjectActivityGenerator { get; set; }
    }
}