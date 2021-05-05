using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Features.GithubIntegration.GithubIntegration;
using MediatR;

namespace Iwentys.Features.Guilds.Tournaments
{
    public class GetTournamentById
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int tournamentId)
            {
                User = user;
                TournamentId = tournamentId;
            }

            public AuthorizedUser User { get; set; }
            public int TournamentId { get; set; }
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
                TournamentInfoResponse result = _tournamentRepository
                    .Get()
                    .Select(TournamentInfoResponse.FromEntity)
                    .Single(t => t.Id == request.TournamentId);

                return new Response(result.OrderByRate());
            }
        }
    }
}