using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.PeerReview;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.PeerReview;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);
            ProjectReviewRequest projectReviewRequest = await _context.ProjectReviewRequests.GetById(request.ReviewRequestId);

            projectReviewRequest.FinishReview(user);

            _context.ProjectReviewRequests.Update(projectReviewRequest);
            return new Response();
        }
    }
}