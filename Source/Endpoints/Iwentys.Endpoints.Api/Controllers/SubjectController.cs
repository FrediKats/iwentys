using System.Collections.Generic;
using Iwentys.Core.Services;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.Api.Controllers
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
        public ActionResult<List<SubjectEntity>> GetAllSubjects([FromQuery] int? courseId, [FromQuery] StudySemester? semester)
        {
            return Ok(_studyLeaderboardService.GetSubjectsForDto(new StudySearchParameters
            {
                CourseId = courseId,
                StudySemester = semester
            }));
        }
    }
}