using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.SubjectAssignments.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    //TODO: merge controllers
    [Route("api/SubjectAssignmentSubmit")]
    [ApiController]
    public class SubjectAssignmentSubmitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectAssignmentSubmitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> GetById(int subjectId, int subjectAssignmentSubmitId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentSubmit.Response response = await _mediator.Send(new GetSubjectAssignmentSubmit.Query(subjectAssignmentSubmitId, authorizedUser));
            return Ok(response.Submit);
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> GetBySubjectId(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetStudentSubjectAssignmentSubmits.Response response = await _mediator.Send(new GetStudentSubjectAssignmentSubmits.Query(new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = subjectId,
                StudentId = authorizedUser.Id
            }, authorizedUser));
            return Ok(response.Submits);
        }

        [HttpPost(nameof(SendSubmit))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> SendSubmit(SubjectAssignmentSubmitCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SendSubmit.Response response = await _mediator.Send(new SendSubmit.Query(arguments, authorizedUser));
            return Ok(response.Submit);
        }

        [HttpGet(nameof(SearchSubjectAssignmentSubmits))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> SearchSubjectAssignmentSubmits(int subjectId, [FromQuery] int? studentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SearchSubjectAssignmentSubmits.Response response = await _mediator.Send(new SearchSubjectAssignmentSubmits.Query(new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = subjectId,
                StudentId = studentId
            }, authorizedUser));

            return Ok(response.Submits);
        }

        [HttpPut(nameof(SendFeedback))]
        public async Task<ActionResult> SendFeedback(SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SendFeedback.Response response = await _mediator.Send(new SendFeedback.Query(arguments, authorizedUser));
            return Ok();
        }
    }
}