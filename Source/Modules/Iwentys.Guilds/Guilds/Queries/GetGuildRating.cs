using System.Collections.Generic;
using System.Linq;
using Iwentys.DataAccess;
using MediatR;

namespace Iwentys.Guilds
{
    public class GetGuildRating
    {
        public class Query : IRequest<Response>
        {
            public Query(int skip, int take)
            {
                Skip = skip;
                Take = take;
            }

            public int Skip { get; set; }
            public int Take { get; set; }
        }

        public class Response
        {
            public Response(List<GuildProfileDto> guilds)
            {
                Guilds = guilds;
            }

            public List<GuildProfileDto> Guilds { get; set; }
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
                List<GuildProfileDto> result = _context
                    .Guilds
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .Select(GuildProfileDto.FromEntity)
                    .ToList()
                    .OrderByDescending(g => g.GuildRating)
                    .ToList();

                return new Response(result);
            }
        }
    }
}