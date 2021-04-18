using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.Extended.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Extended.Newsfeeds
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

        [HttpPost(nameof(CreateSubjectNewsfeed))]
        public async Task<ActionResult> CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _newsfeedService.CreateSubjectNewsfeed(createViewModel, authorizedUser, subjectId);
            return Ok();
        }

        [HttpPost(nameof(CreateGuildNewsfeed))]
        public async Task<ActionResult> CreateGuildNewsfeed(NewsfeedCreateViewModel createViewModel, int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _newsfeedService.CreateGuildNewsfeed(createViewModel, authorizedUser, subjectId);
            return Ok();
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<NewsfeedViewModel>>> GetBySubjectId(int subjectId)
        {
            return Ok(await _newsfeedService.GetSubjectNewsfeeds(subjectId));
        }

        [HttpGet(nameof(GetByGuildId))]
        public async Task<ActionResult<List<NewsfeedViewModel>>> GetByGuildId(int guildId)
        {
            return Ok(await _newsfeedService.GetGuildNewsfeeds(guildId));
        }
    }
}