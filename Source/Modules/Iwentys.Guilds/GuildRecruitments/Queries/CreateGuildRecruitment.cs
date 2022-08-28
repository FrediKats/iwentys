using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public static class CreateGuildRecruitment
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser AuthorizedUser { get; set; }
        public int GuildId { get; set; }
        public GuildRecruitmentCreateArguments Arguments { get; set; }

        public Query(AuthorizedUser authorizedUser, int guildId, GuildRecruitmentCreateArguments arguments)
        {
            AuthorizedUser = authorizedUser;
            GuildId = guildId;
            Arguments = arguments;
        }
    }

    public class Response
    {
        public Response(GuildRecruitmentInfoDto guildRecruitment)
        {
            GuildRecruitment = guildRecruitment;
        }

        public GuildRecruitmentInfoDto GuildRecruitment { get; set; }
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
            Guild guild = _context.Guilds.GetById(request.GuildId).Result;
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            var guildRecruitment = GuildRecruitment.Create(user, guild, request.Arguments);

            _context.GuildRecruitment.Add(guildRecruitment);
            return new Response(GuildRecruitmentInfoDto.FromEntity.Compile().Invoke(guildRecruitment));
        }
    }
}