using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public IwentysDbContext Context { get; }
        public IStudentRepository Student { get; }
        public IStudyGroupRepository StudyGroupRepository { get; }
        public IGuildRepository GuildRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public ITournamentRepository TournamentRepository { get; }
        public IStudentProjectRepository StudentProjectRepository { get; }
        public ITributeRepository TributeRepository { get; }
        public IBarsPointTransactionLogRepository BarsPointTransactionLogRepository { get; }
        public IQuestRepository QuestRepository { get; }
        public IGithubUserDataRepository GithubUserDataRepository { get; }
        public IGuildTestTaskSolvingInfoRepository GuildTestTaskSolvingInfo { get; }


        public ISubjectActivityRepository SubjectActivity { get; }
        public ISubjectForGroupRepository SubjectForGroup { get; }

        public DatabaseAccessor(IwentysDbContext context) : this(
            context,
            new StudentRepository(context),
            new GuildRepository(context), 
            new CompanyRepository(context), 
            new TournamentRepository(context), 
            new StudentProjectRepository(context), 
            new TributeRepository(context), 
            new BarsPointTransactionLogRepository(context), 
            new QuestRepository(context), 
            new SubjectActivityRepository(context), 
            new SubjectForGroupRepository(context),
            new StudyGroupRepository(context),
            new GithubUserDataRepository(context),
            new GuildTestTaskSolvingInfoRepository(context))
        {
        }

        public DatabaseAccessor(IwentysDbContext context,
            IStudentRepository student,
            IGuildRepository guildRepository,
            ICompanyRepository companyRepository,
            ITournamentRepository tournamentRepository,
            IStudentProjectRepository studentProjectRepository,
            ITributeRepository tributeRepository,
            IBarsPointTransactionLogRepository barsPointTransactionLogRepository,
            IQuestRepository questRepository,
            ISubjectActivityRepository subjectActivity,
            ISubjectForGroupRepository subjectForGroup,
            IStudyGroupRepository studyGroupRepository,
            IGithubUserDataRepository githubUserDataRepository,
            IGuildTestTaskSolvingInfoRepository guildTestTaskSolvingInfo)
        {
            Context = context;
            Student = student;
            GuildRepository = guildRepository;
            CompanyRepository = companyRepository;
            TournamentRepository = tournamentRepository;
            StudentProjectRepository = studentProjectRepository;
            TributeRepository = tributeRepository;
            BarsPointTransactionLogRepository = barsPointTransactionLogRepository;
            QuestRepository = questRepository;
            SubjectActivity = subjectActivity;
            SubjectForGroup = subjectForGroup;
            StudyGroupRepository = studyGroupRepository;
            GithubUserDataRepository = githubUserDataRepository;
            GuildTestTaskSolvingInfo = guildTestTaskSolvingInfo;
        }
    }
}