using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Features.Newsfeeds.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsfeedController : ControllerBase
    {
        private readonly NewsfeedService _newsfeedService;

        public NewsfeedController(NewsfeedService newsfeedService)
        {
            _newsfeedService = newsfeedService;
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<ActionResult<List<NewsfeedInfoResponse>>> GetForSubject(int subjectId)
        {
            return Ok(await _newsfeedService.GetSubjectNewsfeedsAsync(subjectId));
        }

        [HttpGet("guild/{guildId}")]
        public async Task<ActionResult<List<NewsfeedInfoResponse>>> GetForGuild(int guildId)
        {
            return Ok(await _newsfeedService.GetGuildNewsfeeds(guildId));
        }
    }
}