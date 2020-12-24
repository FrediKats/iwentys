using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Repositories.Study;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public DatabaseAccessor(IwentysDbContext context) : this(
            context,
            new GuildRepository(context),
            new SubjectActivityRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            GuildRepository guild,
            SubjectActivityRepository subjectActivity)
        {
            Context = context;
            Guild = guild;
            SubjectActivity = subjectActivity;
        }

        public IwentysDbContext Context { get; }
        public GuildRepository Guild { get; }

        public SubjectActivityRepository SubjectActivity { get; }
    }
}