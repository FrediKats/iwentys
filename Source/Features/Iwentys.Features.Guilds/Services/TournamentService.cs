using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Features.GithubIntegration.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class TournamentService
    {
        private readonly AchievementProvider _achievementProvider;
        private readonly IGenericRepository<CodeMarathonTournament> _codeMarathonTournamentRepository;
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<Guild> _guildRepository;

        private readonly IGenericRepository<IwentysUser> _studentRepository;
        private readonly IGenericRepository<Tournament> _tournamentRepository;
        private readonly IGenericRepository<TournamentParticipantTeam> _tournamentTeamRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TournamentService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork, AchievementProvider achievementProvider)
        {
            _unitOfWork = unitOfWork;
            _achievementProvider = achievementProvider;

            _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _tournamentRepository = _unitOfWork.GetRepository<Tournament>();
            _tournamentTeamRepository = _unitOfWork.GetRepository<TournamentParticipantTeam>();
            _codeMarathonTournamentRepository = _unitOfWork.GetRepository<CodeMarathonTournament>();
            _githubIntegrationService = githubIntegrationService;
        }

        public async Task<TournamentInfoResponse> Get(int tournamentId)
        {
            TournamentInfoResponse result = await _tournamentRepository
                .Get()
                .Select(TournamentInfoResponse.FromEntity)
                .SingleAsync(t => t.Id == tournamentId);

            return result.OrderByRate();
        }

        public async Task<TournamentInfoResponse> CreateCodeMarathon(AuthorizedUser user, CreateCodeMarathonTournamentArguments arguments)
        {
            IwentysUser iwentysUser = await _studentRepository.GetById(user.Id);

            var codeMarathonTournamentEntity = CodeMarathonTournament.Create(iwentysUser, arguments);

            _tournamentRepository.Insert(codeMarathonTournamentEntity.Tournament);
            _codeMarathonTournamentRepository.Insert(codeMarathonTournamentEntity);
            await _unitOfWork.CommitAsync();

            return await Get(codeMarathonTournamentEntity.Id);
        }

        public async Task RegisterToTournament(AuthorizedUser user, int tournamentId)
        {
            IwentysUser studentEntity = await _studentRepository.GetById(user.Id);
            Guild guild = _guildMemberRepository.ReadForStudent(user.Id);
            //TODO: check guild for null
            GuildMentor guildMentorUser = studentEntity.EnsureIsGuildMentor(guild);
            Tournament tournamentEntity = await _tournamentRepository.GetById(tournamentId);
            List<GuildMember> members = await _guildRepository
                .Get()
                .SelectMany(g => g.Members)
                .ToListAsync();

            TournamentParticipantTeam tournamentParticipantTeamEntity = tournamentEntity.RegisterTeam(guildMentorUser.Guild, members);

            _tournamentTeamRepository.Insert(tournamentParticipantTeamEntity);
            await _unitOfWork.CommitAsync();
        }
    }
}