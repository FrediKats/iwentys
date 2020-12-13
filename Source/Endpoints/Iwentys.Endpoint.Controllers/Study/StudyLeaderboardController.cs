using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyLeaderboardController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;
        private readonly SubjectActivityService _subjectActivityService;

        public StudyLeaderboardController(StudyLeaderboardService studyLeaderboardService, SubjectActivityService subjectActivityService)
        {
            _studyLeaderboardService = studyLeaderboardService;
            _subjectActivityService = subjectActivityService;
        }

        [HttpGet("study-rate")]
        [Produces("application/json")]
        public ActionResult<List<StudyLeaderboardRowDto>> GetStudyRating(
            [FromQuery] int? subjectId,
            [FromQuery] int? courseId,
            [FromQuery] int? groupId,
            [FromQuery] StudySemester? semester,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 20)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchParametersDto(groupId, subjectId, courseId, semester, skip, take)));
        }

        [HttpGet("coding-rate")]
        [Produces("application/json")]
        public ActionResult<List<StudyLeaderboardRowDto>> GetCodingRating([FromQuery] int? courseId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            return Ok(_studyLeaderboardService.GetCodingRating(courseId, skip, take));
        }
        
        [HttpGet("activity/{studentId}")]
        public async Task<ActionResult<StudentActivityInfoDto>> GetStudentActivity(int studentId)
        {
            List<SubjectActivityEntity> activity = await _subjectActivityService.GetStudentActivity(studentId);
            return new StudentActivityInfoDto(activity);
        }
    }
}
