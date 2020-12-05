using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Models;
using Iwentys.Features.StudentFeature.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyGroupController : ControllerBase
    {
        private readonly StudyGroupService _studyGroupService;
        private readonly StudyLeaderboardService _studyLeaderboardService;

        public StudyGroupController(StudyLeaderboardService studyLeaderboardService, StudyGroupService studyGroupService)
        {
            _studyLeaderboardService = studyLeaderboardService;
            _studyGroupService = studyGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudyGroupEntity>>> GetAllGroups([FromQuery] int? courseId)
        {
            List<StudyGroupEntity> result = await _studyLeaderboardService.GetStudyGroupsForDtoAsync(courseId);
            return Ok(result);
        }

        [HttpGet("by-name/{groupName}")]
        public async Task<ActionResult<GroupProfileResponse>> Get([FromRoute] string groupName)
        {
            GroupProfileResponse result = await _studyGroupService.Get(groupName);
            return Ok(result);
        }
    }
}