using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildTestTasks.Queries
{
    public static class AcceptGuildTestTask
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId, AuthorizedUser user)
            {
                GuildId = guildId;
                User = user;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
        }

        public class Response
        {
            public Response(GuildTestTaskInfoResponse testTaskInfo)
            {
                TestTaskInfo = testTaskInfo;
            }

            public GuildTestTaskInfoResponse TestTaskInfo { get; set; }
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
                IwentysUser author = await _context.IwentysUsers.GetById(request.User.Id);
                Guild authorGuild = await _context.GuildMembers.ReadForStudent(request.User.Id);
                if (authorGuild is null || authorGuild.Id != request.GuildId)
                    throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, request.GuildId);

                var testTaskSolution = GuildTestTaskSolution.Create(authorGuild, author);

                _context.GuildTestTaskSolvingInfos.Add(testTaskSolution);
                return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
            }
        }
    }
}