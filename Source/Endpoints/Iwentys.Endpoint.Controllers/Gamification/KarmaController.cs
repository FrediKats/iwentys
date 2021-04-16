using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.Gamification.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Gamification
{
    [Route("api/karma")]
    [ApiController]
    public class KarmaController : ControllerBase
    {
        private readonly KarmaService _karmaService;

        public KarmaController(KarmaService karmaService)
        {
            _karmaService = karmaService;
        }

        [HttpGet(nameof(GetStatistic))]
        public async Task<ActionResult<KarmaStatistic>> GetStatistic(int studentId)
        {
            KarmaStatistic karmaStatistic = await _karmaService.GetStatistic(studentId);
            return Ok(karmaStatistic);
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