using System.Threading.Tasks;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Infrastructure.Application;
using Iwentys.Modules.SubjectAssignments.Dtos;
using Iwentys.Modules.SubjectAssignments.StudentScope.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.SubjectAssignments.StudentScope
{
    [Route("api/subject-assignment/student/Submit")]
    [ApiController]
    public class StudentSubjectAssignmentSubmitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentSubjectAssignmentSubmitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(CreateSubmit))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> CreateSubmit(SubjectAssignmentSubmitCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubmit.Response response = await _mediator.Send(new CreateSubmit.Query(authorizedUser, arguments));
            return Ok(response.Submit);
        }
    }
}