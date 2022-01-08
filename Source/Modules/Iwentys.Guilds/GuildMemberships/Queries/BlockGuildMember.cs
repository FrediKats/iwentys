using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Guilds
{
    public class BlockGuildMember
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
            public int MemberId { get; set; }

            public Query(AuthorizedUser user, int guildId, int memberId)
            {
                User = user;
                GuildId = guildId;
                MemberId = memberId;
            }
        }

        public class Response
        {

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
                IwentysUser editorStudentAccount = await _context.IwentysUsers.GetById(request.User.Id);
                Guild guild = await _context.Guilds.GetById(request.GuildId);
                GuildMember memberToKick = guild.EnsureMemberCanRestrictPermissionForOther(editorStudentAccount, request.MemberId);
                IwentysUser iwentysUser = await _context.IwentysUsers.GetById(request.User.Id);
                GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _context.GuildLastLeaves);

                memberToKick.MarkBlocked(guildLastLeave);

                _context.GuildMembers.Update(memberToKick);
                return new Response();
            }
        }
    }
}