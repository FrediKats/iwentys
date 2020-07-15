using Iwentys.Database.Repositories.Abstractions;

namespace Iwentys.Database.Context
{
    public class DatabaseAccessor
    {
        public IwentysDbContext Context { get; }
        public IStudentRepository Student { get; }
        public IGuildRepository GuildRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public ITournamentRepository TournamentRepository { get; }
        public IStudentProjectRepository StudentProjectRepository { get; }
        public ITributeRepository TributeRepository { get; }
        public IBarsPointTransactionLogRepository BarsPointTransactionLogRepository { get; }
        public IQuestRepository QuestRepository { get; }

        public DatabaseAccessor(IwentysDbContext context,
            IStudentRepository student,
            IGuildRepository guildRepository,
            ICompanyRepository companyRepository,
            ITournamentRepository tournamentRepository,
            IStudentProjectRepository studentProjectRepository,
            ITributeRepository tributeRepository,
            IBarsPointTransactionLogRepository barsPointTransactionLogRepository,
            IQuestRepository questRepository)
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
        }
    }
}