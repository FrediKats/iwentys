using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using MediatR;

namespace Iwentys.Features.Guilds.Tournaments
{
    public static class FindGuildActiveTournament
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int guildId)
            {
                User = user;
                GuildId = guildId;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
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
            private readonly IGenericRepository<TournamentParticipantTeam> _tournamentTeamRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _tournamentTeamRepository = unitOfWork.GetRepository<TournamentParticipantTeam>();
            }

            protected override Response Handle(Query request)
            {
                TournamentInfoResponse result = _tournamentTeamRepository
                    .Get()
                    .Where(tt => tt.GuildId == request.GuildId)
                    .Select(tt => tt.Tournament)
                    .Select(TournamentInfoResponse.FromEntity)
                    .FirstOrDefault();

                return new Response(result?.OrderByRate());
            }
        }
    }
}