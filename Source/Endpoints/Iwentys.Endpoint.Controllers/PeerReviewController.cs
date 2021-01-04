using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Models;
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

        [HttpGet("requests")]
        public async Task<ActionResult<List<ProjectReviewRequestInfoDto>>> Get()
        {
            List<ProjectReviewRequestInfoDto> result = await _projectReviewService.GetRequests();
            return Ok(result);
        }

        [HttpGet("requests/available-projects")]
        public async Task<ActionResult<List<GithubRepositoryInfoDto>>> GetAvailableForReviewProject()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<GithubRepositoryInfoDto> projects = await _projectReviewService.GetAvailableForReviewProject(authorizedUser);
            return Ok(projects);
        }

        [HttpPost("requests")]
        public async Task<ActionResult> CreateReviewRequest([FromBody] ReviewRequestCreateArguments createArguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _projectReviewService.CreateReviewRequest(authorizedUser, createArguments);
            return Ok();
        }

        [HttpPost("requests/{reviewRequestId}/feedback")]
        public async Task<ActionResult> SendReviewFeedback(int reviewRequestId, [FromBody] ReviewFeedbackCreateArguments createArguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _projectReviewService.SendReviewFeedback(authorizedUser, reviewRequestId, createArguments);
            return Ok();
        }
    }
}