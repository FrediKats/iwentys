using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.PeerReview.Services
{
    public class ProjectReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<ProjectReviewRequest> _projectReviewRequestRepository;
        private readonly IGenericRepository<ProjectReviewFeedback> _projectReviewFeedbackRepository;
        private readonly IGenericRepository<GithubProject> _projectRepository;

        public ProjectReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _projectReviewRequestRepository = _unitOfWork.GetRepository<ProjectReviewRequest>();
            _projectReviewFeedbackRepository = _unitOfWork.GetRepository<ProjectReviewFeedback>();
            _projectRepository = _unitOfWork.GetRepository<GithubProject>();
        }

        public async Task<List<ProjectReviewRequestInfoDto>> GetRequests()
        {
            return await _projectReviewRequestRepository
                .Get()
                .Select(prr => new ProjectReviewRequestInfoDto(prr))
                .ToListAsync();
        }

        public async Task<List<GithubRepositoryInfoDto>> GetAvailableForReviewProject(AuthorizedUser user)
        {
            //TODO: filter project that already on review
            return await _projectRepository
                .Get()
                .Where(p => p.OwnerUserId == user.Id)
                .Select(GithubRepositoryInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<ProjectReviewRequestInfoDto> CreateReviewRequest(AuthorizedUser author, ReviewRequestCreateArguments createArguments)
        {
            //TODO: ensure project was not been added to review before

            var projectReviewRequest = ProjectReviewRequest.Create(author, createArguments);

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

            return new ProjectReviewFeedbackInfoDto(projectReviewFeedback);
        }
    }
}