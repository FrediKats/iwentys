using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using MediatR;

namespace Iwentys.Features.Extended.PeerReview
{
    public class CreateReviewRequest
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, ReviewRequestCreateArguments arguments)
            {
                AuthorizedUser = authorizedUser;
                Arguments = arguments;
            }

            public AuthorizedUser AuthorizedUser { get; set; }
            public ReviewRequestCreateArguments Arguments { get; set; }
        }

        public class Response
        {
            public ProjectReviewRequestInfoDto Result { get; }

            public Response(ProjectReviewRequestInfoDto result)
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
                GithubProject githubProject = await _projectRepository.GetById(request.Arguments.ProjectId);
                IwentysUser user = await _userRepository.GetById(request.AuthorizedUser.Id);

                var projectReviewRequest = ProjectReviewRequest.Create(user, new GithubRepositoryInfoDto(githubProject), request.Arguments);

                projectReviewRequest = _projectReviewRequestRepository.Insert(projectReviewRequest);
                await _unitOfWork.CommitAsync();
                ProjectReviewRequestInfoDto result = _projectReviewRequestRepository.Get().Select(ProjectReviewRequestInfoDto.FromEntity).First(p => p.Id == projectReviewRequest.Id);

                return new Response(result);
            }
        }
    }
}