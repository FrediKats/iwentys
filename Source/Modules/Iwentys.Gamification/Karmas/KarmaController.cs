using System.Threading.Tasks;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Gamification
{
    [Route("api/karma")]
    [ApiController]
    public class KarmaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KarmaController(IMediator mediator)
        {
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
            SendKarma.Response response = await _mediator.Send(new SendKarma.Query(studentId, authorizedUser));
            return Ok();
        }

        [HttpDelete(nameof(Revoke))]
        public async Task<ActionResult> Revoke(int studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            RevokeKarma.Response response = await _mediator.Send(new RevokeKarma.Query(studentId, authorizedUser));
            return Ok();
        }
    }
}