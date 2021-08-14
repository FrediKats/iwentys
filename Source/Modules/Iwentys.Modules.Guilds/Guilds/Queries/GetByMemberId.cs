using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Guilds;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Guilds
{
    public class GetByMemberId
    {
        public class Query : IRequest<Response>
        {
            public Query(int memberId)
            {
                MemberId = memberId;
            }

            public int MemberId { get; set; }
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
                GuildProfileDto guild = _context.GuildMembers.ReadForStudent(request.MemberId).Maybe(g => new GuildProfileDto(g));
                return new Response(guild);
            }
        }
    }
}