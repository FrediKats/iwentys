using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public class AcceptGuildRequest
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }
        public int MemberForAccepting { get; set; }

        public Query(AuthorizedUser user, int guildId, int memberForAccepting)
        {
            User = user;
            GuildId = guildId;
            MemberForAccepting = memberForAccepting;
        }
    }

    public class Response
    {
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, IwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUserInfoDto user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id, cancellationToken);
            Guild guild = await _context.Guilds.GetById(request.GuildId);
            GuildMember member = guild.Members.Find(m => m.MemberId == request.MemberForAccepting) ?? throw EntityNotFoundException.Create(typeof(GuildMember), request.MemberForAccepting);

            GuildMentor guildMentor = GuildMentorUserExtensions.EnsureIsGuildMentor(guild, user.Id);
            member.Approve(guildMentor);

            _context.GuildMembers.Update(member);
            return new Response();
        }
    }
}