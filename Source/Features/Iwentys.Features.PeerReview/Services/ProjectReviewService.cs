using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.PeerReview.Services
{
    public class ProjectReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<ProjectReviewRequest> _projectReviewRequestRepository;

        public ProjectReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _projectReviewRequestRepository = _unitOfWork.GetRepository<ProjectReviewRequest>();
        }

        public async Task<List<ProjectReviewRequestInfoDto>> GetRequests()
        {
            return await _projectReviewRequestRepository
                .Get()
                .Select(prr => new ProjectReviewRequestInfoDto(prr))
                .ToListAsync();
        }

        public async Task<ProjectReviewRequestInfoDto> CreateReviewRequest(AuthorizedUser author, ReviewRequestCreateArguments createArguments)
        {
            //TODO: ensure project was not been added to review before

            var projectReviewRequest = ProjectReviewRequest.Create(author, createArguments);

            await _projectReviewRequestRepository.InsertAsync(projectReviewRequest);
            await _unitOfWork.CommitAsync();

            return new ProjectReviewRequestInfoDto(projectReviewRequest);
        }
    }
}