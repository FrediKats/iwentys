using System.Collections.Generic;
using System.Linq;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Guilds
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
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                List<TournamentInfoResponse> result = _context
                    .Tournaments
                    .Select(TournamentInfoResponse.FromEntity)
                    .ToList();

                result.ForEach(r => r.OrderByRate());
                return new Response(result);
            }
        }
    }
}