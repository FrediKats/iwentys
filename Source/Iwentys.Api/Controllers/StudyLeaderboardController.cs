using System.Collections.Generic;
using Iwentys.Core.Services;
using Iwentys.Models;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;

using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyLeaderboardController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;

        public StudyLeaderboardController(StudyLeaderboardService studyLeaderboardService)
        {
            _studyLeaderboardService = studyLeaderboardService;
        }

        [HttpGet("study-rate")]
        public ActionResult<List<StudyLeaderboardRow>> GetStudyRating([FromQuery] int? subjectId, [FromQuery] int? courseId, [FromQuery] int? groupId, [FromQuery] StudySemester? semester)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchParameters
            {
                SubjectId = subjectId,
                CourseId = courseId,
                GroupId = groupId,
                StudySemester = semester
            }));
        }

        [HttpGet("coding-rate")]
        public ActionResult<List<StudyLeaderboardRow>> GetCodingRating([FromQuery] int? courseId)
        {
            return Ok(_studyLeaderboardService.GetCodingRating(courseId));
        }
    }
}
