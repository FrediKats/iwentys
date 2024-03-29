﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.PeerReview;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.PeerReview;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            GithubProject githubProject = await _context.StudentProjects.GetById(request.Arguments.ProjectId);
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            var projectReviewRequest = ProjectReviewRequest.Create(user, githubProject, request.Arguments);

            EntityEntry<ProjectReviewRequest> createRequest = _context.ProjectReviewRequests.Add(projectReviewRequest);
                
            ProjectReviewRequestInfoDto result = _context
                .ProjectReviewRequests
                .Select(ProjectReviewRequestInfoDto.FromEntity)
                .First(p => p.Id == createRequest.Entity.Id);

            return new Response(result);
        }
    }
}