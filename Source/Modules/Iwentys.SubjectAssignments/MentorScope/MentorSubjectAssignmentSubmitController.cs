using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.SubjectAssignments
{
    [Route("api/subject-assignment/mentor/submit")]
    [ApiController]
    public class MentorSubjectAssignmentSubmitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MentorSubjectAssignmentSubmitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(SearchSubjectAssignmentSubmits))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> SearchSubjectAssignmentSubmits(SubjectAssignmentSubmitSearchArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SearchSubjectAssignmentSubmits.Response response = await _mediator.Send(new SearchSubjectAssignmentSubmits.Query(arguments, authorizedUser));
            return Ok(response.Submits);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> GetById(int subjectAssignmentSubmitId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentSubmit.Response response = await _mediator.Send(new GetSubjectAssignmentSubmit.Query(subjectAssignmentSubmitId, authorizedUser));
            return Ok(response.Submit);
        }

        [HttpPut(nameof(SendSubmitFeedback))]
        public async Task<ActionResult> SendSubmitFeedback(SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SendSubmitFeedback.Response response = await _mediator.Send(new SendSubmitFeedback.Query(authorizedUser, arguments));
            return Ok();
        }
    }
}
