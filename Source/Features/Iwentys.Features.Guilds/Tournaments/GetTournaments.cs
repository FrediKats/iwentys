using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;

namespace Iwentys.Features.Guilds.Tournaments
{
    public class GetTournaments
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user)
            {
                User = user;
            }

            public AuthorizedUser User { get; set; }
        }

        public class Response
        {
            public Response(List<TournamentInfoResponse> tournaments)
            {
                Tournaments = tournaments;
            }

            public List<TournamentInfoResponse> Tournaments { get; set; }
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
                List<TournamentInfoResponse> result = _tournamentRepository
                    .Get()
                    .Select(TournamentInfoResponse.FromEntity)
                    .ToList();

                result.ForEach(r => r.OrderByRate());
                return new Response(result);
            }
        }
    }
}