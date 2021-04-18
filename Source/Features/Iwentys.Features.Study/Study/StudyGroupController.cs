using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Study.Study
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

        [HttpGet(nameof(GetByCourseId))]
        public async Task<ActionResult<List<GroupProfileResponseDto>>> GetByCourseId([FromQuery] int? courseId)
        {
            List<GroupProfileResponseDto> result = await _studyService.GetStudyGroupsForDto(courseId);
            return Ok(result);
        }

        [HttpGet(nameof(GetByGroupName))]
        public async Task<ActionResult<GroupProfileResponseDto>> GetByGroupName(string groupName)
        {
            GroupProfileResponseDto result = await _studyService.GetStudyGroup(groupName);
            return Ok(result);
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<GroupProfileResponseDto>> GetByStudentId(int studentId)
        {
            GroupProfileResponseDto result = await _studyService.GetStudentStudyGroup(studentId);
            if (result is null)
                return NotFound();
            
            return Ok(result);
        }

        [HttpGet(nameof(MakeGroupAdmin))]
        public async Task<ActionResult> MakeGroupAdmin(int newGroupAdminId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _studyService.MakeGroupAdmin(authorizedUser, newGroupAdminId);
            return Ok();
        }
    }
}