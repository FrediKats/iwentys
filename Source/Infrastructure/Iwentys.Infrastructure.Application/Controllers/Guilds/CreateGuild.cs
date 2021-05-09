using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Guilds
{
    public static class CreateGuild
    {
        public class Query : IRequest<Response>
        {
            public GuildCreateRequestDto Arguments { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }

            public Query(GuildCreateRequestDto arguments, AuthorizedUser authorizedUser)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }
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
                IwentysUser creator = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                Guild userCurrentGuild = _context.GuildMembers.ReadForStudent(creator.Id);

                var createdGuild = Guild.Create(creator, userCurrentGuild, request.Arguments);

                _context.Guilds.Add(createdGuild);
                return new Response(new GuildProfileShortInfoDto(createdGuild));
            }
        }
    }
}