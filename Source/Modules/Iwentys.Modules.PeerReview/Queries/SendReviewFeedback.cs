using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.PeerReview.Dtos;
using MediatR;

namespace Iwentys.Modules.PeerReview.Queries
{
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

            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                ProjectReviewRequest projectReviewRequest = await _context.ProjectReviewRequests.GetById(request.ReviewRequestId);

                ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(user, request.Arguments);

                _context.ProjectReviewFeedbacks.Add(projectReviewFeedback);
                ProjectReviewFeedbackInfoDto result = _mapper.Map<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>(projectReviewFeedback);

                return new Response(result);
            }
        }
    }
}