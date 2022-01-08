using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Guilds
{
    public class GetGuildMemberLeaderBoard
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
            public Response(GuildMemberLeaderBoardDto guildMemberLeaderBoard)
            {
                GuildMemberLeaderBoard = guildMemberLeaderBoard;
            }

            public GuildMemberLeaderBoardDto GuildMemberLeaderBoard { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Guild guild = await _context.Guilds.GetById(request.GuildId);

                //return new Response(new GuildMemberLeaderBoardDto(guild.GetImpact()));
                return new Response(new GuildMemberLeaderBoardDto(new List<GuildMemberImpactDto>()));
            }
        }
    }
}