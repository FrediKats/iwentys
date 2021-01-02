using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Gamification.Models;
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

        [HttpGet("{studentId}")]
        public async Task<ActionResult<KarmaStatistic>> GetUserKarmaStatistic(int studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            KarmaStatistic karmaStatistic = await _karmaService.GetStatistic(studentId);
            return Ok(karmaStatistic);
        }

        [HttpPut("{studentId}")]
        public async Task<ActionResult> PutUserKarmaStatistic(int studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _karmaService.UpVote(authorizedUser, studentId);
            return Ok();
        }

        [HttpDelete("{studentId}")]
        public async Task<ActionResult> DeleteUserKarmaStatistic(int studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _karmaService.RemoveUpVote(authorizedUser, studentId);
            return Ok();
        }
    }
}