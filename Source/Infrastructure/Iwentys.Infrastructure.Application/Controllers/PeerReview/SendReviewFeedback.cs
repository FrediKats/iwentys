using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.PeerReview
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

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                ProjectReviewRequest projectReviewRequest = await _context.ProjectReviewRequests.GetById(request.ReviewRequestId);

                ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(request.AuthorizedUser, request.Arguments);

                _context.ProjectReviewFeedbacks.Add(projectReviewFeedback);
                ProjectReviewFeedbackInfoDto result = ProjectReviewFeedbackInfoDto.FromEntity.Compile().Invoke(projectReviewFeedback);

                return new Response(result);
            }
        }
    }
}