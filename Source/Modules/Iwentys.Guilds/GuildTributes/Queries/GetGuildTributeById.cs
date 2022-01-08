using System.Linq;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Guilds
{
    public class GetGuildTributeById
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, int tributeId)
            {
                User = user;
                TributeId = tributeId;
            }

            public AuthorizedUser User { get; set; }
            public int TributeId { get; set; }
        }

        public class Response
        {
            public Response(TributeInfoResponse tribute)
            {
                Tribute = tribute;
            }

            public TributeInfoResponse Tribute { get; set; }
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
                Guild guild = _context.GuildMembers.ReadForStudent(request.User.Id).Result ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, null);

                TributeInfoResponse result = _context
                    .Tributes
                    .Where(t => t.ProjectId == request.TributeId)
                    .Select(TributeInfoResponse.FromEntity)
                    .FirstAsync().Result;

                return new Response(result);
            }
        }
    }
}