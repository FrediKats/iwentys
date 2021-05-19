using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildMemberships
{
    public class GetUserMembership
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
            public Response(UserMembershipState result)
            {
                Result = result;
            }

            public UserMembershipState Result{ get; set; }
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
                Guild guild = await _context.Guilds.GetById(request.GuildId);
                IwentysUser user1 = await _context.IwentysUsers.GetById(request.User.Id);
                GuildLastLeave guildLastLeave = await GuildRepository.Get(user1, _context.GuildLastLeaves);
                GuildMember guildMember = _context
                    .GuildMembers
                    .FirstOrDefault(m => m.Member.Id == request.User.Id && m.MemberType == GuildMemberType.Requested);

                UserMembershipState result = guild.GetUserMembershipState(user1, guildMember, guildLastLeave);
                return new Response(result);
            }
        }
    }
}