using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Services
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
            HashSet<long> userProjects = _projectReviewRequestRepository
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
            IwentysUser user = await _userRepository.GetById(author.Id);

            var projectReviewRequest = ProjectReviewRequest.Create(user, new GithubRepositoryInfoDto(githubProject), createArguments);

            projectReviewRequest = _projectReviewRequestRepository.Insert(projectReviewRequest);
            await _unitOfWork.CommitAsync();
            return _projectReviewRequestRepository.Get().Select(ProjectReviewRequestInfoDto.FromEntity).First(p => p.Id == projectReviewRequest.Id);
        }

        public async Task<ProjectReviewFeedbackInfoDto> SendReviewFeedback(AuthorizedUser author, int reviewRequestId, ReviewFeedbackCreateArguments createArguments)
        {
            ProjectReviewRequest projectReviewRequest = await _projectReviewRequestRepository.GetById(reviewRequestId);

            ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(author, createArguments);

            projectReviewFeedback = _projectReviewFeedbackRepository.Insert(projectReviewFeedback);
            await _unitOfWork.CommitAsync();
            return ProjectReviewFeedbackInfoDto.FromEntity.Compile().Invoke(projectReviewFeedback);
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

            projectReviewRequestInvite = _projectReviewRequestInviteRepository.Insert(projectReviewRequestInvite);
            await _unitOfWork.CommitAsync();
        }
    }
}