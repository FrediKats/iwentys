using System.Collections.Generic;
using Iwentys.Core.Services;
using Iwentys.Models;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Study
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
        public ActionResult<List<StudyLeaderboardRow>> GetStudyRating(
            [FromQuery] int? subjectId,
            [FromQuery] int? courseId,
            [FromQuery] int? groupId,
            [FromQuery] StudySemester? semester,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 20)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchParameters
            {
                SubjectId = subjectId,
                CourseId = courseId,
                GroupId = groupId,
                StudySemester = semester,
                Skip = skip,
                Take = take
            }));
        }

        [HttpGet("coding-rate")]
        public ActionResult<List<StudyLeaderboardRow>> GetCodingRating([FromQuery] int? courseId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return Ok(_studyLeaderboardService.GetCodingRating(courseId, skip, take));
        }
    }
}
