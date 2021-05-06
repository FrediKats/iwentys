using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using MediatR;

namespace Iwentys.Infrastructure.Application.PeerReview
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
                ProjectReviewRequest projectReviewRequest = await _projectReviewRequestRepository.GetById(request.ReviewRequestId);

                ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(request.AuthorizedUser, request.Arguments);

                projectReviewFeedback = _projectReviewFeedbackRepository.Insert(projectReviewFeedback);
                await _unitOfWork.CommitAsync();
                ProjectReviewFeedbackInfoDto result = ProjectReviewFeedbackInfoDto.FromEntity.Compile().Invoke(projectReviewFeedback);

                return new Response(result);
            }
        }
    }
}