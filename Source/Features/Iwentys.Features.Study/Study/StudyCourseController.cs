using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Models;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Study.Study
{
    [Route("api/study-courses")]
    [ApiController]
    public class StudyCourseController : ControllerBase
    {
        private readonly StudyService _studyService;

        public StudyCourseController(StudyService studyService)
        {
            _studyService = studyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudyCourseInfoDto>>> Get()
        {
            List<StudyCourseInfoDto> studyCourses = await _studyService.GetStudyCourses();
            return Ok(studyCourses);
        }
    }
}