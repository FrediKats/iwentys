using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyGroupController : ControllerBase
    {
        private readonly StudyService _studyService;

        public StudyGroupController(StudyService studyService)
        {
            _studyService = studyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GroupProfileResponseDto>>> GetCourseGroups([FromQuery] int? courseId)
        {
            List<GroupProfileResponseDto> result = await _studyService.GetStudyGroupsForDto(courseId);
            return Ok(result);
        }

        [HttpGet("by-name/{groupName}")]
        public async Task<ActionResult<GroupProfileResponseDto>> Get([FromRoute] string groupName)
        {
            GroupProfileResponseDto result = await _studyService.GetStudyGroup(groupName);
            return Ok(result);
        }

        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<GroupProfileResponseDto>> GetStudentGroup(int studentId)
        {
            GroupProfileResponseDto result = await _studyService.GetStudentStudyGroup(studentId);
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpGet("promote-admin/{newGroupAdminId}")]
        public async Task<ActionResult> MakeGroupAdmin(int newGroupAdminId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _studyService.MakeGroupAdmin(authorizedUser, newGroupAdminId);
            return Ok();
        }
    }
}