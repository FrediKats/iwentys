using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.PeerReview.Queries
{
    public class FinishReview
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, int reviewRequestId)
            {
                AuthorizedUser = authorizedUser;
                ReviewRequestId = reviewRequestId;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public int ReviewRequestId { get; set; }
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
                IwentysUser user = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);
                ProjectReviewRequest projectReviewRequest = await _context.ProjectReviewRequests.GetById(request.ReviewRequestId);

                projectReviewRequest.FinishReview(user);

                _context.ProjectReviewRequests.Update(projectReviewRequest);
                return new Response();
            }
        }
    }
}