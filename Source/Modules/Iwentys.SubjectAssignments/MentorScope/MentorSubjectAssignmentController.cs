using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.SubjectAssignments
{
    [Route("api/subject-assignment/mentor")]
    [ApiController]
    public class MentorSubjectAssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MentorSubjectAssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(SubjectAssignmentCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubjectAssignment.Response response = await _mediator.Send(new CreateSubjectAssignment.Query(authorizedUser, arguments));
            return Ok();
        }

        [HttpPost(nameof(Update))]
        public async Task<ActionResult> Update(SubjectAssignmentUpdateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            UpdateSubjectAssignment.Response response = await _mediator.Send(new UpdateSubjectAssignment.Query(authorizedUser, arguments));
            return Ok(response.SubjectAssignment);
        }

        [HttpPost(nameof(Delete))]
        public async Task<ActionResult> Delete(int subjectAssignmentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            DeleteSubjectAssignment.Response response = await _mediator.Send(new DeleteSubjectAssignment.Query(authorizedUser, subjectAssignmentId));
            return Ok(response.SubjectAssignment);
        }

        [HttpPost(nameof(Recover))]
        public async Task<ActionResult> Recover(int subjectAssignmentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            RecoverSubjectAssignment.Response response = await _mediator.Send(new RecoverSubjectAssignment.Query(authorizedUser, subjectAssignmentId));
            return Ok(response.SubjectAssignment);
        }

        [HttpGet(nameof(GetMentorSubjectAssignments))]
        [Produces("application/json")]
        public async Task<ActionResult<List<SubjectAssignmentJournalItemDto>>> GetMentorSubjectAssignments()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetMentorSubjectAssignments.Response response = await _mediator.Send(new GetMentorSubjectAssignments.Query(authorizedUser));
            return Ok(response.SubjectAssignments);
        }
    }
}
