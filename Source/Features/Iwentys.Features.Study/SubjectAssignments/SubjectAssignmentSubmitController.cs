using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/SubjectAssignmentSubmit")]
    [ApiController]
    public class SubjectAssignmentSubmitController : ControllerBase
    {
        private readonly SubjectAssignmentService _subjectAssignmentService;

        public SubjectAssignmentSubmitController(SubjectAssignmentService subjectAssignmentService)
        {
            _subjectAssignmentService = subjectAssignmentService;
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> GetById(int subjectId, int subjectAssignmentSubmitId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SubjectAssignmentSubmitDto result = await _subjectAssignmentService.GetSubjectAssignmentSubmit(authorizedUser, subjectAssignmentSubmitId);
            return Ok(result);
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> GetBySubjectId(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            List<SubjectAssignmentSubmitDto> submits = await _subjectAssignmentService.GetStudentSubjectAssignmentSubmits(authorizedUser, new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = subjectId,
                StudentId = authorizedUser.Id
            });
            return Ok(submits);
        }

        [HttpPost(nameof(SendSubmit))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> SendSubmit(int subjectId, int subjectAssignmentId, SubjectAssignmentSubmitCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SubjectAssignmentSubmitDto result = await _subjectAssignmentService.SendSubmit(authorizedUser, subjectAssignmentId, arguments);
            return Ok(result);
        }

        [HttpGet(nameof(SearchSubjectAssignmentSubmits))]
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

        [HttpPut(nameof(SendFeedback))]
        public async Task<ActionResult> SendFeedback(int subjectId, int subjectAssignmentSubmitId, [FromBody] SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _subjectAssignmentService.SendFeedback(authorizedUser, subjectAssignmentSubmitId, arguments);
            return Ok();
        }
    }
}