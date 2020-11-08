using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Core.Services;
using Iwentys.Models.Entities.Study;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.OldServer.Controllers
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
        public async Task<ActionResult<List<StudyGroupEntity>>> GetAllGroups([FromQuery] int? courseId)
        {
            List<StudyGroupEntity> result = await _studyLeaderboardService.GetStudyGroupsForDtoAsync(courseId);
            return Ok(result);
        }
    }
}