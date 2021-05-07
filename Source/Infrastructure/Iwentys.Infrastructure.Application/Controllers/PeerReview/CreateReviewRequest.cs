using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Infrastructure.Application.Controllers.PeerReview
{
    public class CreateReviewRequest
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, ReviewRequestCreateArguments arguments)
            {
                AuthorizedUser = authorizedUser;
                Arguments = arguments;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public ReviewRequestCreateArguments Arguments { get; set; }
        }

        public class Response
        {
            public ProjectReviewRequestInfoDto Result { get; }

            public Response(ProjectReviewRequestInfoDto result)
            {
                Result = result;
            }
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
                GithubProject githubProject = await _context.StudentProjects.GetById(request.Arguments.ProjectId);
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                var projectReviewRequest = ProjectReviewRequest.Create(user, new GithubRepositoryInfoDto(githubProject), request.Arguments);

                EntityEntry<ProjectReviewRequest> createRequest = _context.ProjectReviewRequests.Add(projectReviewRequest);
                
                ProjectReviewRequestInfoDto result = _context
                    .ProjectReviewRequests
                    .Select(ProjectReviewRequestInfoDto.FromEntity)
                    .First(p => p.Id == createRequest.Entity.Id);

                return new Response(result);
            }
        }
    }
}