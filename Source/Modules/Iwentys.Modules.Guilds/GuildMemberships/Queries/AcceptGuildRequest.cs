using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildMemberships.Queries
{
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

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser student = await _context.IwentysUsers.GetById(request.User.Id);
                Guild guild = await _context.Guilds.GetById(request.GuildId);
                GuildMember member = guild.Members.Find(m => m.MemberId == request.MemberForAccepting) ?? throw EntityNotFoundException.Create(typeof(GuildMember), request.MemberForAccepting);

                GuildMentor guildMentor = student.EnsureIsGuildMentor(guild);
                member.Approve(guildMentor);

                _context.GuildMembers.Update(member);
                return new Response();
            }
        }
    }
}