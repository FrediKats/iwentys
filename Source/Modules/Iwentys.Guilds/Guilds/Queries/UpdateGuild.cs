using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Guild guild = await _context.Guilds.GetById(request.Arguments.Id);
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            guild.Update(user, request.Arguments);

            _context.Guilds.Update(guild);
            return new Response(new GuildProfileShortInfoDto(guild));
        }
    }
}