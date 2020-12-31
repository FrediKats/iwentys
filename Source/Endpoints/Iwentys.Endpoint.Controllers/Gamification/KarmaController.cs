using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Gamification.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Gamification
{
    [Route("api/leaderboard")]
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
            KarmaStatistic karmaStatistic = await _karmaService.GetStatistic(studentId);
            return Ok(karmaStatistic);
        }

    }
}