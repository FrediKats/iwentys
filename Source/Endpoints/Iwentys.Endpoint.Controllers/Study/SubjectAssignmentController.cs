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
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetAllGroups(int groupSubjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentDto> result = await _subjectAssignmentService.GetSubjectAssignmentForGroup(authorizedUser, groupSubjectId);
            return Ok(result);
        }
    }
}