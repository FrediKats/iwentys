using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Enums;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Gamification
{
    //TODO: refactor naming
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;
        private readonly SubjectActivityService _subjectActivityService;

        public LeaderboardController(StudyLeaderboardService studyLeaderboardService, SubjectActivityService subjectActivityService)
        {
            _studyLeaderboardService = studyLeaderboardService;
            _subjectActivityService = subjectActivityService;
        }

        [HttpGet("study")]
        public ActionResult<List<StudyLeaderboardRowDto>> GetStudyRating(
            int? subjectId,
            int? courseId,
            int? groupId,
            StudySemester? semester,
            int skip = 0,
            int take = 20)
        {
            return Ok(_studyLeaderboardService.GetStudentsRatings(new StudySearchParametersDto(groupId, subjectId, courseId, semester, skip, take)));
        }

        //FYI: test propose only. Need to move to daemon
        [HttpGet("force-update")]
        public async Task<ActionResult> CourseRatingForceRefresh(int courseId)
        {
            await _studyLeaderboardService.CourseRatingForceRefresh(courseId);
            return Ok();
        }

        [HttpGet("student-position")]
        public async Task<ActionResult<CourseLeaderboardRow>> FindStudentLeaderboardPosition(int studentId)
        {
            CourseLeaderboardRow position = await _studyLeaderboardService.FindStudentLeaderboardPosition(studentId);
            if (position is null)
                return NotFound();
            return Ok(position);
        }

        [HttpGet("coding")]
        public ActionResult<List<StudyLeaderboardRowDto>> GetCodingRating([FromQuery] int? courseId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            List<StudyLeaderboardRowDto> result = _studyLeaderboardService.GetCodingRating(courseId, skip, take);
            return Ok(result);
        }
        
        [HttpGet("activity/{studentId}")]
        public async Task<ActionResult<StudentActivityInfoDto>> GetStudentActivity(int studentId)
        {
            List<SubjectActivity> activity = await _subjectActivityService.GetStudentActivity(studentId);
            return new StudentActivityInfoDto(activity);
        }
    }
}
