using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Guilds
{
    public class GetGuildById
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
            public Response(GuildProfileDto guild)
            {
                Guild = guild;
            }

            public GuildProfileDto Guild { get; set; }
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
                GuildProfileDto guildProfileDto = await _context
                    .Guilds
                    .Where(g => g.Id == request.GuildId)
                    .Select(GuildProfileDto.FromEntity)
                    .SingleAsync(cancellationToken);

                return new Response(guildProfileDto);
            }
        }
    }
}