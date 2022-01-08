using System.Collections.Generic;
using System.Linq;
using Iwentys.DataAccess;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Guilds
{
    public static class GetGuildTestTaskSubmits
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId)
            {
                GuildId = guildId;
            }

            public int GuildId { get; set; }
        }

        public class Response
        {
            public Response(List<GuildTestTaskInfoResponse> submits)
            {
                Submits = submits;
            }

            public List<GuildTestTaskInfoResponse> Submits { get; set; }
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
                List<GuildTestTaskInfoResponse> result = _context
                    .GuildTestTaskSolvingInfos
                    .Where(t => t.GuildId == request.GuildId)
                    .Select(GuildTestTaskInfoResponse.FromEntity)
                    .ToListAsync().Result;

                return new Response(result);
            }
        }
    }
}