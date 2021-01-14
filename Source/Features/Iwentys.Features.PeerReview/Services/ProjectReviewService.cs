using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.PeerReview.Services
{
    public class ProjectReviewService
    {
        private readonly IGenericRepository<GithubProject> _projectRepository;
        private readonly IGenericRepository<ProjectReviewFeedback> _projectReviewFeedbackRepository;
        private readonly IGenericRepository<ProjectReviewRequest> _projectReviewRequestRepository;
        private readonly IGenericRepository<ProjectReviewRequestInvite> _projectReviewRequestInviteRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _userRepository;

        public ProjectReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _projectReviewRequestRepository = _unitOfWork.GetRepository<ProjectReviewRequest>();
            _projectReviewFeedbackRepository = _unitOfWork.GetRepository<ProjectReviewFeedback>();
            _projectRepository = _unitOfWork.GetRepository<GithubProject>();
            _projectReviewRequestInviteRepository = _unitOfWork.GetRepository<ProjectReviewRequestInvite>();
        }

        public async Task<List<ProjectReviewRequestInfoDto>> GetRequests(AuthorizedUser user)
        {
            return await _projectReviewRequestRepository
                .Get()
                .Where(ProjectReviewRequest.IsVisibleTo(user))
                .Select(ProjectReviewRequestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<GithubRepositoryInfoDto>> GetAvailableForReviewProject(AuthorizedUser user)
         {
            var userProjects = _projectReviewRequestRepository
                .Get()
                .Where(k => k.AuthorId == user.Id)
                .SelectToHashSet(k => k.ProjectId);

            return await _projectRepository
                .Get()
                .Where(p => p.OwnerUserId == user.Id && !userProjects.Contains(p.Id))
                .Select(GithubRepositoryInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<ProjectReviewRequestInfoDto> CreateReviewRequest(AuthorizedUser author, ReviewRequestCreateArguments createArguments)
        {
            GithubProject githubProject = await _projectRepository.GetById(createArguments.ProjectId);
            var alreadyAddedToReview = _projectReviewRequestRepository.Get().Any(rr => rr.ProjectId == githubProject.Id);
            if (alreadyAddedToReview)
                throw InnerLogicException.PeerReviewExceptions.ProjectAlreadyOnReview(githubProject.Id);

            var projectReviewRequest = ProjectReviewRequest.Create(author, githubProject, createArguments);

            await _projectReviewRequestRepository.InsertAsync(projectReviewRequest);
            await _unitOfWork.CommitAsync();

            return await _projectReviewRequestRepository
                .Get()
                .Where(rr => rr.Id == projectReviewRequest.Id)
                .Select(ProjectReviewRequestInfoDto.FromEntity)
                .SingleOrDefaultAsync();
        }

        public async Task<ProjectReviewFeedbackInfoDto> SendReviewFeedback(AuthorizedUser author, int reviewRequestId, ReviewFeedbackCreateArguments createArguments)
        {
            ProjectReviewRequest projectReviewRequest = await _projectReviewRequestRepository.GetById(reviewRequestId);
            ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(author, createArguments);

            await _projectReviewFeedbackRepository.InsertAsync(projectReviewFeedback);
            await _unitOfWork.CommitAsync();
            return await _projectReviewFeedbackRepository
                .Get()
                .Where(f => f.Id == projectReviewFeedback.Id)
                .Select(ProjectReviewFeedbackInfoDto.FromEntity)
                .SingleAsync();
        }

        public async Task FinishReview(AuthorizedUser authorizedUser, int reviewRequestId)
        {
            IwentysUser user = await _userRepository.GetById(authorizedUser.Id);
            ProjectReviewRequest projectReviewRequest = await _projectReviewRequestRepository.GetById(reviewRequestId);

            projectReviewRequest.FinishReview(user);

            _projectReviewRequestRepository.Update(projectReviewRequest);
            await _unitOfWork.CommitAsync();
        }

        public async Task InviteToReview(AuthorizedUser requestAuthor, int reviewRequestId, int reviewToInviteId)
        {
            IwentysUser user = await _userRepository.GetById(requestAuthor.Id);
            ProjectReviewRequest projectReviewRequest = await _projectReviewRequestRepository.GetById(reviewRequestId);
            IwentysUser reviewToInvite = await _userRepository.GetById(reviewToInviteId);

            ProjectReviewRequestInvite projectReviewRequestInvite = projectReviewRequest.InviteToReview(user, reviewToInvite);

            await _projectReviewRequestInviteRepository.InsertAsync(projectReviewRequestInvite);
            await _unitOfWork.CommitAsync();
        }
    }
}