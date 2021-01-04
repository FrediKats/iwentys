using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.PeerReview.Models;
using Iwentys.Features.PeerReview.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/peer-review")]
    [ApiController]
    public class PeerReviewController : ControllerBase
    {
        private readonly ProjectReviewService _projectReviewService;

        public PeerReviewController(ProjectReviewService projectReviewService)
        {
            _projectReviewService = projectReviewService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectReviewRequestInfoDto>>> Get()
        {
            List<ProjectReviewRequestInfoDto> result = await _projectReviewService.GetRequests();
            return Ok(result);
        }
    }
}