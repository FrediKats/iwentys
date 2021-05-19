using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildMemberships
{
    public class RejectGuildRequest
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
                IwentysUser initiator = await _context.IwentysUsers.GetById(request.User.Id);
                Guild guild = await _context.Guilds.GetById(request.GuildId);
                IwentysUser iwentysUser = await _context.IwentysUsers.GetById(request.MemberId);
                GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _context.GuildLastLeaves);

                GuildMember member = guild.Members.Find(m => m.MemberId == request.MemberId);

                if (member is null || member.MemberType != GuildMemberType.Requested)
                    throw InnerLogicException.GuildExceptions.RequestWasNotFound(request.MemberId, request.GuildId);

                guild.RemoveMember(initiator, iwentysUser, guildLastLeave);
                return new Response();
            }
        }
    }
}