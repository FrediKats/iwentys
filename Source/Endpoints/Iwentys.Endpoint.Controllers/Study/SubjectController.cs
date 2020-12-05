using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
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
        public async Task<ActionResult<List<SubjectProfileDto>>> GetAllSubjects([FromQuery] int? courseId, [FromQuery] StudySemester? semester)
        {
            var studySearchParameters = new StudySearchParametersDto(null, null, courseId, semester, 0, 20);
            List<SubjectEntity> subjectInfo = await _subjectService.GetSubjectsForDtoAsync(studySearchParameters);

            List<SubjectProfileDto> response = subjectInfo.SelectToList(entity => new SubjectProfileDto(entity));
            return Ok(response);
        }

        [HttpGet("profile/{subjectId}")]
        public async Task<ActionResult<SubjectProfileDto>> GetProfile([FromRoute] int subjectId)
        {
            SubjectProfileDto subject = await _subjectService.Get(subjectId);
            return Ok(subject);
        }

        [HttpGet("search/for-group")]
        public async Task<ActionResult<List<SubjectProfileDto>>> GetGroupSubjects(int groupId)
        {
            return Ok(await _subjectService.GetGroupSubjects(groupId));
        }
    }
}