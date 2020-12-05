using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Features.Newsfeeds.ViewModels;
using Iwentys.Features.StudentFeature;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
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

        [HttpPost("subject/{subjectId}")]
        public async Task<ActionResult<NewsfeedViewModel>> CreateSubject(NewsfeedCreateViewModel createViewModel, int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            return Ok(await _newsfeedService.CreateSubjectNewsfeed(createViewModel, authorizedUser.Id, subjectId));
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<ActionResult<List<NewsfeedViewModel>>> GetForSubject(int subjectId)
        {
            return Ok(await _newsfeedService.GetSubjectNewsfeedsAsync(subjectId));
        }

        [HttpGet("guild/{guildId}")]
        public async Task<ActionResult<List<NewsfeedViewModel>>> GetForGuild(int guildId)
        {
            return Ok(await _newsfeedService.GetGuildNewsfeeds(guildId));
        }
    }
}