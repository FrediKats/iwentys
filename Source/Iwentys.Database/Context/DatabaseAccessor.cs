using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Repositories.Study;
using Iwentys.Features.Guilds.Repositories;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public DatabaseAccessor(IwentysDbContext context) : this(
            context,
            new GuildRepository(context),
            new GuildMemberRepository(context),
            new SubjectActivityRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            GuildRepository guild,
            IGuildMemberRepository guildMember,
            SubjectActivityRepository subjectActivity)
        {
            Context = context;
            Guild = guild;
            GuildMember = guildMember;
            SubjectActivity = subjectActivity;
        }

        public IwentysDbContext Context { get; }
        public GuildRepository Guild { get; }
        public IGuildMemberRepository GuildMember { get; }

        public SubjectActivityRepository SubjectActivity { get; }
    }
}