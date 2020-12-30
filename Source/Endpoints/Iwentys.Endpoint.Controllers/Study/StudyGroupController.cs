using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyGroupController : ControllerBase
    {
        private readonly StudyGroupService _studyGroupService;

        public StudyGroupController(StudyGroupService studyGroupService)
        {
            _studyGroupService = studyGroupService;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudyGroup>>> GetAllGroups([FromQuery] int? courseId)
        {
            List<StudyGroup> result = await _studyGroupService.GetStudyGroupsForDtoAsync(courseId);
            return Ok(result);
        }

        [HttpGet("by-name/{groupName}")]
        public async Task<ActionResult<GroupProfileResponseDto>> Get([FromRoute] string groupName)
        {
            GroupProfileResponseDto result = await _studyGroupService.Get(groupName);
            return Ok(result);
        }

        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<GroupProfileResponseDto>> GetStudentGroup(int studentId)
        {
            GroupProfileResponseDto result = await _studyGroupService.GetStudentGroup(studentId);
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }
    }
}