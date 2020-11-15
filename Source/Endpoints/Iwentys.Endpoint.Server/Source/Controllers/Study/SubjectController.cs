using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Core.Services;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;

        public SubjectController(StudyLeaderboardService studyLeaderboardService)
        {
            _studyLeaderboardService = studyLeaderboardService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubjectEntity>>> GetAllSubjects([FromQuery] int? courseId, [FromQuery] StudySemester? semester)
        {
            List<SubjectEntity> result = await _studyLeaderboardService.GetSubjectsForDtoAsync(new StudySearchParameters
            {
                CourseId = courseId,
                StudySemester = semester
            });

            return Ok(result);
        }
    }
}