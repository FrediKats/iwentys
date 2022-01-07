using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildMemberships.Queries
{
    public class LeaveGuild
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

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser iwentysUser = await _context.IwentysUsers.GetById(request.User.Id);
                GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _context.GuildLastLeaves);

                Guild studentGuild = await _context.GuildMembers.ReadForStudent(request.User.Id);
                if (studentGuild is null || studentGuild.Id != request.GuildId)
                    throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, request.GuildId);

                //TributeEntity userTribute = _guildTributeRepository.Get()
                //    .Where(t => t.GuildId == guildId)
                //    .Where(t => t.ProjectEntity.AuthorId == user.Id)
                //    .SingleOrDefault(t => t.State == TributeState.Active);

                //if (userTribute is not null)
                //    await _guildTributeRepository.DeleteAsync(userTribute.ProjectId);

                studentGuild.RemoveMember(iwentysUser, iwentysUser, guildLastLeave);
                return new Response();
            }
        }
    }
}