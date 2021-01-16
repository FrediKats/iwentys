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

        [HttpGet("{subjectId}/submits/{subjectAssignmentSubmitId}")]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> GetSubjectAssignmentSubmit(int subjectId, int subjectAssignmentSubmitId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SubjectAssignmentSubmitDto result = await _subjectAssignmentService.GetSubjectAssignmentSubmit(authorizedUser, subjectAssignmentSubmitId);
            return Ok(result);
        }

        [HttpGet("{subjectId}/submits")]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> GetStudentSubjectAssignmentSubmits(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentSubmitDto> submits = await _subjectAssignmentService.GetStudentSubjectAssignmentSubmits(authorizedUser, new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = subjectId,
                StudentId = authorizedUser.Id
            });
            return Ok(submits);
        }

        [HttpPost("{subjectId}/assignments/{subjectAssignmentId}/submits")]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> SendSubmit(int subjectId, int subjectAssignmentId, SubjectAssignmentSubmitCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SubjectAssignmentSubmitDto result = await _subjectAssignmentService.SendSubmit(authorizedUser, subjectAssignmentId, arguments);
            return Ok(result);
        }

        #region Teacher only method

        [HttpPost("management/{subjectId}")]
        public async Task<ActionResult> CreateSubjectAssignment(int subjectId, SubjectAssignmentCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _subjectAssignmentService.CreateSubjectAssignment(authorizedUser, subjectId, arguments);
            return Ok();
        }

        [HttpGet("management/{subjectId}/submits")]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> SearchSubjectAssignmentSubmits(int subjectId, [FromQuery] int? studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentSubmitDto> submits = await _subjectAssignmentService.SearchSubjectAssignmentSubmits(authorizedUser, new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = subjectId,
                StudentId = studentId
            });
            return Ok(submits);
        }

        [HttpPut("management/{subjectId}/submits/{subjectAssignmentSubmitId}")]
        public async Task<ActionResult> SendFeedback(int subjectId, int subjectAssignmentSubmitId, [FromBody] SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _subjectAssignmentService.SendFeedback(authorizedUser, subjectAssignmentSubmitId, arguments);
            return Ok();
        }

        #endregion
    }
}