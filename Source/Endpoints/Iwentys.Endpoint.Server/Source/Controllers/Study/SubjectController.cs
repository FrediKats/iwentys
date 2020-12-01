using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Enums;
using Iwentys.Features.StudentFeature.Services;
using Iwentys.Features.StudentFeature.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly StudyLeaderboardService _studyLeaderboardService;
        private readonly SubjectService _subjectService;

        public SubjectController(StudyLeaderboardService studyLeaderboardService, SubjectService subjectService)
        {
            _studyLeaderboardService = studyLeaderboardService;
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

            List<SubjectProfileResponse> response = subjectInfo.SelectToList(SubjectProfileResponse.Wrap);
            return Ok(response);
        }

        [HttpGet("profile/{subjectId}")]
        public async Task<ActionResult<SubjectProfileResponse>> GetProfile([FromRoute] int subjectId)
        {
            SubjectProfileResponse subject = await _subjectService.Get(subjectId);
            return Ok(subject);
        }
    }
}