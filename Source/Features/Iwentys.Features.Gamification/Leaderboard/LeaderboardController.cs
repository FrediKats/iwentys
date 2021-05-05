using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Gamification.Leaderboard
{
    //TODO: refactor naming
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaderboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("study")]
        public async Task<ActionResult<List<StudyLeaderboardRowDto>>> GetStudyRating(
            int? subjectId,
            int? courseId,
            int? groupId,
            StudySemester? semester,
            int skip = 0,
            int take = 20)
        {
            GetStudyRating.Response response = await _mediator.Send(new GetStudyRating.Query(groupId, subjectId, courseId, semester, skip, take));
            return Ok(response.Leaders);
        }

        //FYI: test propose only. Need to move to daemon
        [HttpGet("force-update")]
        public async Task<ActionResult> CourseRatingForceRefresh(int courseId)
        {
            CourseRatingForceRefresh.Response response = await _mediator.Send(new CourseRatingForceRefresh.Query(courseId));
            return Ok();
        }

        [HttpGet("student-position")]
        public async Task<ActionResult<CourseLeaderboardRow>> FindStudentLeaderboardPosition(int studentId)
        {
            FindStudentLeaderboardPosition.Response response = await _mediator.Send(new FindStudentLeaderboardPosition.Query(studentId));

            //TODO: refactor
            if (response.Position is null)
                return NotFound();
            return Ok(response.Position);
        }

        [HttpGet("coding")]
        public async Task<ActionResult<List<StudyLeaderboardRowDto>>> GetCodingRating([FromQuery] int? courseId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            GetCodingRating.Response response = await _mediator.Send(new GetCodingRating.Query(courseId, skip, take));
            return Ok(response.Rating);
        }

        //TODO: move this method to Study controller 
        //[HttpGet("activity/{studentId}")]
        //public async Task<ActionResult<StudentActivityInfoDto>> GetStudentActivity(int studentId)
        //{
        //    GetStudentActivity.Response response = await _mediator.Send(new GetStudentActivity.Query(courseId, skip, take));
        //    List<SubjectActivity> activity = await _subjectActivityService.GetStudentActivity(studentId);
        //    return new StudentActivityInfoDto(activity);
        //}
    }
}
