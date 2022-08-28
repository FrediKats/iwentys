using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

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
        private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, IwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUserInfoDto user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id, cancellationToken);
            IwentysUser author = EntityManagerApiDtoMapper.Map(user);
            Guild authorGuild = await _context.GuildMembers.ReadForStudent(request.User.Id);
            if (authorGuild is null || authorGuild.Id != request.GuildId)
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, request.GuildId);

            var testTaskSolution = GuildTestTaskSolution.Create(authorGuild, author);

            _context.GuildTestTaskSolvingInfos.Add(testTaskSolution);
            return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
        }
    }
}