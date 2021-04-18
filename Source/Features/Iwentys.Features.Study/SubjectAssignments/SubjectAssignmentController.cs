using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Study.SubjectAssignments
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

        [HttpGet(nameof(GetByGroupId))]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetByGroupId(int groupId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentDto> result = await _subjectAssignmentService.GetSubjectAssignmentForGroup(authorizedUser, groupId);
            return Ok(result);
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetBySubjectId(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentDto> result = await _subjectAssignmentService.GetSubjectAssignmentForSubject(authorizedUser, subjectId);
            return Ok(result);
        }
        
        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(int subjectId, SubjectAssignmentCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _subjectAssignmentService.CreateSubjectAssignment(authorizedUser, subjectId, arguments);
            return Ok();
        }
    }
}