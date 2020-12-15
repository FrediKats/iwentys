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
            new GuildTributeRepository(context),
            new SubjectActivityRepository(context),
            new GuildTestTaskSolvingInfoRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            GuildRepository guild,
            IGuildMemberRepository guildMember,
            GuildTributeRepository guildTribute,
            SubjectActivityRepository subjectActivity,
            IGuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfo)
        {
            Context = context;
            Guild = guild;
            GuildMember = guildMember;
            GuildTribute = guildTribute;
            SubjectActivity = subjectActivity;
            GuildTestTaskSolvingInfo = guildTestTaskSolvingInfo;
        }

        public IwentysDbContext Context { get; }
        public GuildRepository Guild { get; }
        public IGuildMemberRepository GuildMember { get; }

        public GuildTributeRepository GuildTribute { get; }
        public IGuildTestTaskSolvingInfoRepository GuildTestTaskSolvingInfo { get; }

        public SubjectActivityRepository SubjectActivity { get; }
    }
}