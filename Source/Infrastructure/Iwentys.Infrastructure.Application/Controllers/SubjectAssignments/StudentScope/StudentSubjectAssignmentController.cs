using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.StudentScope
{
    [Route("api/subject-assignment/student")]
    [ApiController]
    public class StudentSubjectAssignmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentSubjectAssignmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //TODO: add filter and pagination
        [HttpGet(nameof(GetStudentSubjectAssignments))]
        public async Task<ActionResult<List<SubjectAssignmentJournalItemDto>>> GetStudentSubjectAssignments()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetStudentSubjectAssignments.Response response = await _mediator.Send(new GetStudentSubjectAssignments.Query(authorizedUser));
            return Ok(response.SubjectAssignments);
        }

        [HttpGet(nameof(GetStudentSubjectAssignmentSubmits))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> GetStudentSubjectAssignmentSubmits(int subjectAssignmentId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetStudentSubjectAssignmentSubmits.Response response = await _mediator.Send(new GetStudentSubjectAssignmentSubmits.Query(authorizedUser, subjectAssignmentId));
            return Ok(response.SubjectAssignments);
        }
    }
}
