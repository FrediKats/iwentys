using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.SubjectAssignments.Models;
using Iwentys.Features.Study.SubjectAssignments.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/subject-assignment")]
    [ApiController]
    public class SubjectAssignmentController : ControllerBase
    {
        private readonly SubjectAssignmentService _subjectAssignmentService;

        public SubjectAssignmentController(SubjectAssignmentService subjectAssignmentService)
        {
            _subjectAssignmentService = subjectAssignmentService;
        }

        [HttpGet("for-group/{groupId}")]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetAssignmentForGroup(int groupId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentDto> result = await _subjectAssignmentService.GetSubjectAssignmentForGroup(authorizedUser, groupId);
            return Ok(result);
        }

        [HttpGet("for-subject/{subjectId}")]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetSubjectAssignmentForSubject(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentDto> result = await _subjectAssignmentService.GetSubjectAssignmentForSubject(authorizedUser, subjectId);
            return Ok(result);
        }

        [HttpPost("{subjectId}")]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> CreateSubjectAssignment(int subjectId, SubjectAssignmentCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _subjectAssignmentService.CreateSubjectAssignment(authorizedUser, subjectId, arguments);
            return Ok();
        }
    }
}