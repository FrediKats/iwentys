using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.PeerReview.Entities;
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

        public async Task<List<ProjectReviewRequest>> GetRequests()
        {
            return await _projectReviewRequestRepository
                .Get()
                .ToListAsync();
        }
    }
}