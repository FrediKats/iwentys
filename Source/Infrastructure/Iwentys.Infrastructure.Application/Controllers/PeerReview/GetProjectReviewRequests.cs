using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.PeerReview
{
    public class GetProjectReviewRequests
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser)
            {
                AuthorizedUser = authorizedUser;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
        }

        public class Response
        {
            public Response(List<ProjectReviewRequestInfoDto> requests)
            {
                Requests = requests;
            }

            public List<ProjectReviewRequestInfoDto> Requests { get; set; }
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
                List<ProjectReviewRequestInfoDto> result = await _projectReviewRequestRepository
                    .Get()
                    .Where(ProjectReviewRequest.IsVisibleTo(user))
                    .Select(ProjectReviewRequestInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}