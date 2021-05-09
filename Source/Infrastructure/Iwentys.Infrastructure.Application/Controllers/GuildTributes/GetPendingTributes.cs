using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildTributes
{
    public class GetPendingTributes
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
            public Response(List<TributeInfoResponse> tributes)
            {
                Tributes = tributes;
            }

            public List<TributeInfoResponse> Tributes { get; set; }
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
                Guild guild = _context.GuildMembers.ReadForStudent(request.User.Id) ?? throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, null);

                List<TributeInfoResponse> result = _context
                    .Tributes
                    .Where(t => t.GuildId == guild.Id)
                    .Where(t => t.Project.OwnerUserId == request.User.Id)
                    .Select(TributeInfoResponse.FromEntity)
                    .ToList();

                return new Response(result);
            }
        }
    }
}