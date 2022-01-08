using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.PeerReview;
using Iwentys.Infrastructure.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.PeerReview
{
    [Route("api/peer-review")]
    [ApiController]
    public class PeerReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PeerReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet(nameof(GetProjectReviewRequests))]
        public async Task<ActionResult<List<ProjectReviewRequestInfoDto>>> GetProjectReviewRequests()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetProjectReviewRequests.Response response = await _mediator.Send(new GetProjectReviewRequests.Query(authorizedUser));
            return Ok(response.Requests);
        }

        [HttpGet(nameof(GetAvailableForReviewProject))]
        public async Task<ActionResult<List<GithubRepositoryInfoDto>>> GetAvailableForReviewProject()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetAvailableForReviewProject.Response response = await _mediator.Send(new GetAvailableForReviewProject.Query(authorizedUser));
            return Ok(response.Result);
        }

        [HttpPost(nameof(CreateReviewRequest))]
        public async Task<ActionResult> CreateReviewRequest([FromBody] ReviewRequestCreateArguments createArguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateReviewRequest.Response response = await _mediator.Send(new CreateReviewRequest.Query(authorizedUser, createArguments));
            return Ok();
        }

        [HttpPost(nameof(SendReviewFeedback))]
        public async Task<ActionResult> SendReviewFeedback(int reviewRequestId, [FromBody] ReviewFeedbackCreateArguments createArguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SendReviewFeedback.Response response = await _mediator.Send(new SendReviewFeedback.Query(authorizedUser, createArguments, reviewRequestId));

            return Ok();
        }

        [HttpPost(nameof(FinishReview))]
        public async Task<ActionResult> FinishReview(int reviewRequestId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            FinishReview.Response response = await _mediator.Send(new FinishReview.Query(authorizedUser, reviewRequestId));

            return Ok();
        }
    }
}