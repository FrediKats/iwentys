using Iwentys.Database.Repositories.Study;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public DatabaseAccessor(IwentysDbContext context) : this(
            context,
            new SubjectActivityRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            SubjectActivityRepository subjectActivity)
        {
            Context = context;
            SubjectActivity = subjectActivity;
        }

        public IwentysDbContext Context { get; }

        public SubjectActivityRepository SubjectActivity { get; }
    }
}