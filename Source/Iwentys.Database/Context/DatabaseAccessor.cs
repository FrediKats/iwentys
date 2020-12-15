using Iwentys.Database.Repositories.GithubIntegration;
using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Repositories.Study;
using Iwentys.Features.GithubIntegration.Repositories;
using Iwentys.Features.Guilds.Repositories;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public DatabaseAccessor(IwentysDbContext context) : this(
            context,
            new GuildRepository(context),
            new GuildMemberRepository(context),
            new StudentProjectRepository(context),
            new GuildTributeRepository(context),
            new SubjectActivityRepository(context),
            new GroupSubjectRepository(context),
            new GithubUserDataRepository(context),
            new GuildTestTaskSolvingInfoRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            GuildRepository guild,
            IGuildMemberRepository guildMember,
            IStudentProjectRepository studentProject,
            GuildTributeRepository guildTribute,
            SubjectActivityRepository subjectActivity,
            GroupSubjectRepository groupSubject,
            IGithubUserDataRepository githubUserData,
            IGuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfo)
        {
            Context = context;
            Guild = guild;
            GuildMember = guildMember;
            StudentProject = studentProject;
            GuildTribute = guildTribute;
            SubjectActivity = subjectActivity;
            GroupSubject = groupSubject;
            GithubUserData = githubUserData;
            GuildTestTaskSolvingInfo = guildTestTaskSolvingInfo;
        }

        public IwentysDbContext Context { get; }
        public GuildRepository Guild { get; }
        public IGuildMemberRepository GuildMember { get; }

        public IStudentProjectRepository StudentProject { get; }
        public GuildTributeRepository GuildTribute { get; }
        public IGithubUserDataRepository GithubUserData { get; }
        public IGuildTestTaskSolvingInfoRepository GuildTestTaskSolvingInfo { get; }

        public SubjectActivityRepository SubjectActivity { get; }
        public GroupSubjectRepository GroupSubject { get; }
    }
}