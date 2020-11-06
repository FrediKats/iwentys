using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.GithubIntegration;
using Iwentys.Features.GithubIntegration.Repositories;

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
            new TributeRepository(context),
            new BarsPointTransactionLogRepository(context),
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
            GuildMemberRepository guildMember,
            CompanyRepository company,
            TournamentRepository tournament,
            IStudentProjectRepository studentProject,
            TributeRepository tribute,
            BarsPointTransactionLogRepository barsPointTransactionLog,
            QuestRepository quest,
            SubjectActivityRepository subjectActivity,
            GroupSubjectRepository groupSubject,
            StudyGroupRepository studyGroup,
            IGithubUserDataRepository githubUserData,
            GuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfo,
            AssignmentRepository assignment,
            GuildRecruitmentRepository guildRecruitment)
        {
            Context = context;
            Student = student;
            Guild = guild;
            GuildMember = guildMember;
            Company = company;
            Tournament = tournament;
            StudentProject = studentProject;
            Tribute = tribute;
            BarsPointTransactionLog = barsPointTransactionLog;
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
        public GuildMemberRepository GuildMember { get; }
        public GuildRecruitmentRepository GuildRecruitment { get; }

        public CompanyRepository Company { get; }
        public TournamentRepository Tournament { get; }
        public IStudentProjectRepository StudentProject { get; }
        public TributeRepository Tribute { get; }
        public BarsPointTransactionLogRepository BarsPointTransactionLog { get; }
        public QuestRepository Quest { get; }
        public IGithubUserDataRepository GithubUserData { get; }
        public GuildTestTaskSolvingInfoRepository GuildTestTaskSolvingInfo { get; }

        public AssignmentRepository Assignment { get; }

        public SubjectActivityRepository SubjectActivity { get; }
        public GroupSubjectRepository GroupSubject { get; }
    }
}