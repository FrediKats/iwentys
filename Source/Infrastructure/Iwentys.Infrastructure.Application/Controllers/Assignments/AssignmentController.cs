using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.Assignments
{
    [Route("api/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<AssignmentInfoDto>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetStudentAssignment.Response response = await _mediator.Send(new GetStudentAssignment.Query(user));
            return Ok(response.AssignmentInfos);
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<AssignmentInfoDto>> Create([FromBody] AssignmentCreateArguments assignmentCreateArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CreateAssignment.Response response = await _mediator.Send(new CreateAssignment.Query(user, assignmentCreateArguments));
            return Ok(response.AssignmentInfos);
        }

        [HttpGet(nameof(Complete))]
        public async Task<ActionResult> Complete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CompleteAssignment.Response response = await _mediator.Send(new CompleteAssignment.Query(user, assignmentId));
            return Ok();
        }

        [HttpGet(nameof(Undo))]
        public async Task<ActionResult> Undo(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            UndoAssignmentComplete.Response response = await _mediator.Send(new UndoAssignmentComplete.Query(user, assignmentId));
            return Ok();
        }

        [HttpGet(nameof(Delete))]
        public async Task<ActionResult> Delete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            DeleteAssignment.Response response = await _mediator.Send(new DeleteAssignment.Query(user, assignmentId));
            return Ok();
        }
    }
}