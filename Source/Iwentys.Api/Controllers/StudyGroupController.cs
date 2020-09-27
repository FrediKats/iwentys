using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyGroupController : ControllerBase
    {
        private readonly IStudyLeaderboardService _studyLeaderboardService;

        public StudyGroupController(IStudyLeaderboardService studyLeaderboardService)
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