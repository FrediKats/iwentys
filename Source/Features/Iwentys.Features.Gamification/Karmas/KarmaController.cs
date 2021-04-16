using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.FeatureBase;
using Iwentys.Features.Gamification.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Gamification.Karmas
{
    [Route("api/karma")]
    [ApiController]
    public class KarmaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly KarmaService _karmaService;

        public KarmaController(KarmaService karmaService, IMediator mediator)
        {
            _karmaService = karmaService;
            _mediator = mediator;
        }

        [HttpGet(nameof(GetStatistic))]
        public async Task<ActionResult<GetKarmaStatistic.Response>> GetStatistic(int studentId)
        {
            GetKarmaStatistic.Response response = await _mediator.Send(new GetKarmaStatistic.Query(studentId));
            return Ok(response);
        }

        [HttpPut(nameof(Send))]
        public async Task<ActionResult> Send(int studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _karmaService.UpVote(authorizedUser, studentId);
            return Ok();
        }

        [HttpDelete(nameof(Revoke))]
        public async Task<ActionResult> Revoke(int studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _karmaService.RemoveUpVote(authorizedUser, studentId);
            return Ok();
        }
    }
}