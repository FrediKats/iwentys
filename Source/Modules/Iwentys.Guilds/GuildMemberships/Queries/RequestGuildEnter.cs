using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class RequestGuildEnter
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }

        public Query(AuthorizedUser user, int guildId)
        {
            User = user;
            GuildId = guildId;
        }
    }

    public class Response
    {

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
            Guild guild = await _context.Guilds.GetById(request.GuildId);
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
            GuildLastLeave lastLeave = await _context.GuildLastLeaves.FindAsync(user.Id);
            GuildMember guildMember = _context.GuildMembers
                .FirstOrDefault(m => m.Member.Id == request.User.Id && m.MemberType == GuildMemberType.Requested);

            GuildMember newMembership = guild.RequestEnterGuild(user, guildMember, lastLeave);

            _context.GuildMembers.Add(newMembership);
            return new Response();
        }
    }
}