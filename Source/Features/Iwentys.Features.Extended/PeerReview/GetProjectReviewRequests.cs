using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.GithubIntegration;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.PeerReview
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
                List<ProjectReviewRequestInfoDto> result = await _projectReviewRequestRepository
                    .Get()
                    .Where(ProjectReviewRequest.IsVisibleTo(request.AuthorizedUser))
                    .Select(ProjectReviewRequestInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}