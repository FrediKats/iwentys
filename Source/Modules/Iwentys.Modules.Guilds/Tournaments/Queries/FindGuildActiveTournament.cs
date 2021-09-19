using System.Linq;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Guilds.Dtos;
using MediatR;

namespace Iwentys.Modules.Guilds.Tournaments.Queries
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
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                TournamentInfoResponse result = _context
                    .TournamentParticipantTeams
                    .Where(tt => tt.GuildId == request.GuildId)
                    .Select(tt => tt.Tournament)
                    .Select(TournamentInfoResponse.FromEntity)
                    .FirstOrDefault();

                return new Response(result?.OrderByRate());
            }
        }
    }
}