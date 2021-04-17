using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Tournaments
{
    public class CreateCodeMarathon
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, CreateCodeMarathonTournamentArguments arguments)
            {
                User = user;
                Arguments = arguments;
            }

            public AuthorizedUser User { get; set; }
            public CreateCodeMarathonTournamentArguments Arguments { get; set; }
        }

        public class Response
        {
            public Response(TournamentInfoResponse tournament)
            {
                Tournament = tournament;
            }

            public TournamentInfoResponse Tournament { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
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

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService, AchievementProvider achievementProvider)
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

            protected override Response Handle(Query request)
            {
                SystemAdminUser systemAdminUser = (_studentRepository.GetById(request.User.Id).Result).EnsureIsAdmin();

                var codeMarathonTournamentEntity = CodeMarathonTournament.Create(systemAdminUser, request.Arguments);

                _tournamentRepository.InsertAsync(codeMarathonTournamentEntity.Tournament).Wait();
                _codeMarathonTournamentRepository.InsertAsync(codeMarathonTournamentEntity).Wait();
                _unitOfWork.CommitAsync().Wait();

                return new Response(Get(codeMarathonTournamentEntity.Id).Result);
            }

            public async Task<TournamentInfoResponse> Get(int tournamentId)
            {
                TournamentInfoResponse result = await _tournamentRepository
                    .Get()
                    .Select(TournamentInfoResponse.FromEntity)
                    .SingleAsync(t => t.Id == tournamentId);

                return result.OrderByRate();
            }
        }
    }
}