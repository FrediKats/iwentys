using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Models.Entities.Newsfeeds;
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
        public async Task<ActionResult<List<SubjectNewsfeedEntity>>> GetCreatedByUser(int subjectId)
        {
            List<SubjectNewsfeedEntity> result = await _newsfeedService.GetSubjectNewsfeeds(subjectId);
            return Ok(result);
        }
    }
}