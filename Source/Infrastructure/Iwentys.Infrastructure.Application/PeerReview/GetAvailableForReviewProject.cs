using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.PeerReview;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.PeerReview
{
    public class GetAvailableForReviewProject
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
            public List<GithubRepositoryInfoDto> Result { get; set; }

            public Response(List<GithubRepositoryInfoDto> result)
            {
                Result = result;
            }
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
                HashSet<long> userProjects = _projectReviewRequestRepository
                    .Get()
                    .Where(k => k.AuthorId == request.AuthorizedUser.Id)
                    .SelectToHashSet(k => k.ProjectId);

                List<GithubRepositoryInfoDto> result = await _projectRepository
                    .Get()
                    .Where(p => p.OwnerUserId == request.AuthorizedUser.Id && !userProjects.Contains(p.Id))
                    .Select(GithubRepositoryInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}