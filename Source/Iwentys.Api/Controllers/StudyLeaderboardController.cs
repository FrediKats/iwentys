using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyLeaderboardController : ControllerBase
    {
        private readonly IStudyLeaderboardService _studyLeaderboardService;

        public StudyLeaderboardController(IStudyLeaderboardService studyLeaderboardService)
        {
            _studyLeaderboardService = studyLeaderboardService;
        }

        [HttpGet]
        public ActionResult<List<StudyLeaderboardRow>> GetStudentsRating([FromQuery] int? subjectId, [FromQuery] int? courseId, [FromQuery] int? groupId, [FromQuery] StudySemester? semester)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchDto
            {
                SubjectId = subjectId,
                CourseId = courseId,
                GroupId = groupId,
                StudySemester = semester
            }));
        }
    }
}
