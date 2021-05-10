using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Tournaments
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
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                TournamentInfoResponse result = _context
                    .Tournaments
                    .Select(TournamentInfoResponse.FromEntity)
                    .Single(t => t.Id == request.TournamentId);

                return new Response(result.OrderByRate());
            }
        }
    }
}