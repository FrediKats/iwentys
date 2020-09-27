using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public IwentysDbContext Context { get; }
        public IStudentRepository Student { get; }
        public IStudyGroupRepository StudyGroup { get; }
        public IGuildRepository Guild { get; }
        public IGuildMemberRepository GuildMember { get; }
        public ICompanyRepository Company { get; }
        public ITournamentRepository Tournament { get; }
        public IStudentProjectRepository StudentProject { get; }
        public ITributeRepository Tribute { get; }
        public IBarsPointTransactionLogRepository BarsPointTransactionLog { get; }
        public IQuestRepository Quest { get; }
        public IGithubUserDataRepository GithubUserData { get; }
        public IGuildTestTaskSolvingInfoRepository GuildTestTaskSolvingInfo { get; }

        public IAssignmentRepository Assignment { get; }

        public ISubjectActivityRepository SubjectActivity { get; }
        public IGroupSubjectRepository GroupSubject { get; }

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
            new GroupGroupSubjectRepository(context),
            new StudyGroupRepository(context),
            new GithubUserDataRepository(context),
            new GuildTestTaskSolvingInfoRepository(context),
            new AssignmentRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            IStudentRepository student,
            IGuildRepository guild,
            IGuildMemberRepository guildMember,
            ICompanyRepository company,
            ITournamentRepository tournament,
            IStudentProjectRepository studentProject,
            ITributeRepository tribute,
            IBarsPointTransactionLogRepository barsPointTransactionLog,
            IQuestRepository quest,
            ISubjectActivityRepository subjectActivity,
            IGroupSubjectRepository groupSubject,
            IStudyGroupRepository studyGroup,
            IGithubUserDataRepository githubUserData,
            IGuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfo,
            IAssignmentRepository assignment)
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
        }
    }
}