using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.PeerReview;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Guilds;

public static class SubmitGuildTestTask
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user, int guildId, string projectOwner, string projectName)
        {
            User = user;
            GuildId = guildId;
            ProjectOwner = projectOwner;
            ProjectName = projectName;
        }

        public AuthorizedUser User { get; set; }
        public int GuildId { get; set; }
        public string ProjectOwner { get; set; }
        public string ProjectName { get; set; }
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
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, GithubIntegrationService githubIntegrationService, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _githubIntegrationService = githubIntegrationService;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Guild guild = await _context.Guilds.GetById(request.GuildId);
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
            GithubProject githubRepositoryInfoDto = _githubIntegrationService.Repository.GetRepositoryAsProject(request.ProjectOwner, request.ProjectName).Result;
            GuildTestTaskSolution testTaskSolution = await _context.GuildTestTaskSolvingInfos.GetSingle(t => t.AuthorId == request.User.Id && t.GuildId == request.GuildId);

            ProjectReviewRequest reviewRequest = ProjectReviewRequest.CreateGuildReviewRequest(user, githubRepositoryInfoDto, testTaskSolution, guild);

            _context.GuildTestTaskSolvingInfos.Update(testTaskSolution);
            _context.ProjectReviewRequests.Add(reviewRequest);
            return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
        }
    }
}