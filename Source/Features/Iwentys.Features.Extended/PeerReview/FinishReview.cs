using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.GithubIntegration;
using MediatR;

namespace Iwentys.Features.Extended.PeerReview
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
            private readonly IGenericRepository<GithubProject> _projectRepository;
            private readonly IGenericRepository<ProjectReviewFeedback> _projectReviewFeedbackRepository;
            private readonly IGenericRepository<ProjectReviewRequest> _projectReviewRequestRepository;
            private readonly IGenericRepository<ProjectReviewRequestInvite> _projectReviewRequestInviteRepository;
            private readonly IGenericRepository<IwentysUser> _userRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _projectReviewRequestRepository = _unitOfWork.GetRepository<ProjectReviewRequest>();
                _projectReviewFeedbackRepository = _unitOfWork.GetRepository<ProjectReviewFeedback>();
                _projectRepository = _unitOfWork.GetRepository<GithubProject>();
                _projectReviewRequestInviteRepository = _unitOfWork.GetRepository<ProjectReviewRequestInvite>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser user = await _userRepository.GetById(request.AuthorizedUser.Id);
                ProjectReviewRequest projectReviewRequest = await _projectReviewRequestRepository.GetById(request.ReviewRequestId);

                projectReviewRequest.FinishReview(user);

                _projectReviewRequestRepository.Update(projectReviewRequest);
                await _unitOfWork.CommitAsync();
                return new Response();
            }
        }
    }
}