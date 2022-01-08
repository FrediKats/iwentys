using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Guilds
{
    public class UpdateGuild
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, GuildUpdateRequestDto arguments)
            {
                AuthorizedUser = authorizedUser;
                Arguments = arguments;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public GuildUpdateRequestDto Arguments { get; set; }
        }

        public class Response
        {
            public Response(GuildProfileShortInfoDto guild)
            {
                Guild = guild;
            }

            public GuildProfileShortInfoDto Guild { get; set; }
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
                Guild guild = await _context.Guilds.GetById(request.Arguments.Id);
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                guild.Update(user, request.Arguments);

                _context.Guilds.Update(guild);
                return new Response(new GuildProfileShortInfoDto(guild));
            }
        }
    }
}