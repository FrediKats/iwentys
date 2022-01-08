using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.PeerReview;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.Guilds.GuildTestTasks.Queries
{
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

            public Handler(IwentysDbContext context, GithubIntegrationService githubIntegrationService)
            {
                _context = context;
                _githubIntegrationService = githubIntegrationService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Guild guild = await _context.Guilds.GetById(request.GuildId);
                IwentysUser user = await _context.IwentysUsers.GetById(request.User.Id);
                GithubProject githubRepositoryInfoDto = _githubIntegrationService.Repository.GetRepositoryAsProject(request.ProjectOwner, request.ProjectName).Result;
                GuildTestTaskSolution testTaskSolution = await _context.GuildTestTaskSolvingInfos.GetSingle(t => t.AuthorId == request.User.Id && t.GuildId == request.GuildId);

                ProjectReviewRequest reviewRequest = ProjectReviewRequest.CreateGuildReviewRequest(user, githubRepositoryInfoDto, testTaskSolution, guild);

                _context.GuildTestTaskSolvingInfos.Update(testTaskSolution);
                _context.ProjectReviewRequests.Add(reviewRequest);
                return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
            }
        }
    }
}