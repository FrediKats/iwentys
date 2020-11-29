using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Core.Services;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Features.StudentFeature.Services;
using Iwentys.Models;
using Iwentys.Models.Entities.Newsfeeds;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Study;
using Iwentys.Models.Types;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;
        private readonly SubjectService _subjectService;
        private readonly NewsfeedService _newsfeedService;

        public SubjectController(StudyLeaderboardService studyLeaderboardService, NewsfeedService newsfeedService, SubjectService subjectService)
        {
            _studyLeaderboardService = studyLeaderboardService;
            _newsfeedService = newsfeedService;
            _subjectService = subjectService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<SubjectProfileResponse>>> GetAllSubjects([FromQuery] int? courseId, [FromQuery] StudySemester? semester)
        {
            List<SubjectEntity> subjectInfo = await _studyLeaderboardService.GetSubjectsForDtoAsync(new StudySearchParameters
            {
                CourseId = courseId,
                StudySemester = semester
            });

            List<SubjectProfileResponse> response = subjectInfo.SelectToList(entity => SubjectProfileResponse.Wrap(entity, new List<SubjectNewsfeedEntity>()));
            return Ok(response);
        }

        [HttpGet("profile/{subjectId}")]
        public async Task<ActionResult<SubjectProfileResponse>> GetProfile([FromRoute] int subjectId)
        {
            SubjectProfileResponse subject = await _subjectService.Get(subjectId);
            subject.Newsfeeds = await _newsfeedService.GetSubjectNewsfeedsAsync(subjectId);
            return Ok(subject);
        }
    }
}