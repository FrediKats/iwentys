﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.PeerReview;

public class SendReviewFeedback
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser authorizedUser, ReviewFeedbackCreateArguments arguments, int reviewRequestId)
        {
            AuthorizedUser = authorizedUser;
            Arguments = arguments;
            ReviewRequestId = reviewRequestId;
        }

        public AuthorizedUser AuthorizedUser { get; set; }
        public ReviewFeedbackCreateArguments Arguments { get; set; }
        public int ReviewRequestId { get; set; }
    }

    public class Response
    {
        public Response(ProjectReviewFeedbackInfoDto reviewFeedbackInfoDto)
        {
            ReviewFeedbackInfoDto = reviewFeedbackInfoDto;
        }

        public ProjectReviewFeedbackInfoDto ReviewFeedbackInfoDto { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, IMapper mapper, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _mapper = mapper;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);
            ProjectReviewRequest projectReviewRequest = await _context.ProjectReviewRequests.GetById(request.ReviewRequestId);

            ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(user, request.Arguments);

            _context.ProjectReviewFeedbacks.Add(projectReviewFeedback);
            ProjectReviewFeedbackInfoDto result = _mapper.Map<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>(projectReviewFeedback);

            return new Response(result);
        }
    }
}