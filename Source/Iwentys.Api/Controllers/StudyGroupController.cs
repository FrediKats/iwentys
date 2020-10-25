using System.Collections.Generic;
using Iwentys.Core.Services;
using Iwentys.Models.Entities.Study;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyGroupController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;

        public StudyGroupController(StudyLeaderboardService studyLeaderboardService)
        {
            _studyLeaderboardService = studyLeaderboardService;
        }

        [HttpGet]
        public ActionResult<List<StudyGroupEntity>> GetAllGroups([FromQuery] int? courseId)
        {
            return Ok(_studyLeaderboardService.GetStudyGroupsForDto(courseId));
        }
    }
}