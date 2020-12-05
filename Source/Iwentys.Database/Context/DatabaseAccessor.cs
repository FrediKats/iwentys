using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Economy;
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
            new StudentRepository(context),
            new GuildRepository(context),
            new GuildMemberRepository(context),
            new CompanyRepository(context),
            new TournamentRepository(context),
            new StudentProjectRepository(context),
            new GuildTributeRepository(context),
            new BarsPointTransactionRepository(context),
            new QuestRepository(context),
            new SubjectActivityRepository(context),
            new GroupSubjectRepository(context),
            new StudyGroupRepository(context),
            new GithubUserDataRepository(context),
            new GuildTestTaskSolvingInfoRepository(context),
            new AssignmentRepository(context),
            new GuildRecruitmentRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            StudentRepository student,
            GuildRepository guild,
            IGuildMemberRepository guildMember,
            CompanyRepository company,
            TournamentRepository tournament,
            IStudentProjectRepository studentProject,
            GuildTributeRepository guildTribute,
            BarsPointTransactionRepository barsPointTransaction,
            QuestRepository quest,
            SubjectActivityRepository subjectActivity,
            GroupSubjectRepository groupSubject,
            StudyGroupRepository studyGroup,
            IGithubUserDataRepository githubUserData,
            IGuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfo,
            AssignmentRepository assignment,
            IGuildRecruitmentRepository guildRecruitment)
        {
            Context = context;
            Student = student;
            Guild = guild;
            GuildMember = guildMember;
            Company = company;
            Tournament = tournament;
            StudentProject = studentProject;
            GuildTribute = guildTribute;
            BarsPointTransaction = barsPointTransaction;
            Quest = quest;
            SubjectActivity = subjectActivity;
            GroupSubject = groupSubject;
            StudyGroup = studyGroup;
            GithubUserData = githubUserData;
            GuildTestTaskSolvingInfo = guildTestTaskSolvingInfo;
            Assignment = assignment;
            GuildRecruitment = guildRecruitment;
        }

        public IwentysDbContext Context { get; }
        public StudentRepository Student { get; }
        public StudyGroupRepository StudyGroup { get; }
        public GuildRepository Guild { get; }
        public IGuildMemberRepository GuildMember { get; }
        public IGuildRecruitmentRepository GuildRecruitment { get; }

        public CompanyRepository Company { get; }
        public TournamentRepository Tournament { get; }
        public IStudentProjectRepository StudentProject { get; }
        public GuildTributeRepository GuildTribute { get; }
        public BarsPointTransactionRepository BarsPointTransaction { get; }
        public QuestRepository Quest { get; }
        public IGithubUserDataRepository GithubUserData { get; }
        public IGuildTestTaskSolvingInfoRepository GuildTestTaskSolvingInfo { get; }

        public AssignmentRepository Assignment { get; }

        public SubjectActivityRepository SubjectActivity { get; }
        public GroupSubjectRepository GroupSubject { get; }
    }
}