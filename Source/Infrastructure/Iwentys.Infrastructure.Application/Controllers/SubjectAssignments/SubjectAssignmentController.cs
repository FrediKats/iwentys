using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Assignments.Models;
using Iwentys.Domain.SubjectAssignments.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    [Route("api/subject-assignment")]
    [ApiController]
    public class SubjectAssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectAssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetByGroupId))]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetByGroupId(int groupId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentForGroup.Response response = await _mediator.Send(new GetSubjectAssignmentForGroup.Query(groupId));
            return Ok(response.SubjectAssignments);
        }

        [HttpGet(nameof(GetBySubjectId))]
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetBySubjectId(int subjectId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentForSubject.Response response = await _mediator.Send(new GetSubjectAssignmentForSubject.Query(subjectId));
            return Ok(response.SubjectAssignments);
        }
        
        [HttpPost(nameof(Create))]
        public async Task<ActionResult> Create(SubjectAssignmentCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubjectAssignment.Response response = await _mediator.Send(new CreateSubjectAssignment.Query(authorizedUser, arguments));
            return Ok();
        }
    }
}