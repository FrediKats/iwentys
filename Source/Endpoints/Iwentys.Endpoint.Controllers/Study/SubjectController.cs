using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.Enums;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectService _subjectService;

        public SubjectController(SubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<SubjectProfileDto>>> Get(int? courseId, StudySemester? semester)
        {
            var studySearchParameters = new StudySearchParametersDto(null, null, courseId, semester, 0, 20);
            List<SubjectProfileDto> subjectInfo = await _subjectService.GetSubjectsForDto(studySearchParameters);

            return Ok(subjectInfo);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<SubjectProfileDto>> GetById([FromRoute] int subjectId)
        {
            SubjectProfileDto subject = await _subjectService.Get(subjectId);
            return Ok(subject);
        }

        [HttpGet(nameof(GetByGroupId))]
        public async Task<ActionResult<List<SubjectProfileDto>>> GetByGroupId(int groupId)
        {
            List<SubjectProfileDto> result = await _subjectService.GetGroupSubjects(groupId);
            return Ok(result);
        }
    }
}