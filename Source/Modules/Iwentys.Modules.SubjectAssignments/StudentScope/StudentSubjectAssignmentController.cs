using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.SubjectAssignments.Dtos;
using Iwentys.Modules.SubjectAssignments.StudentScope.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.SubjectAssignments.StudentScope
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
        public async Task<ActionResult<List<SubjectAssignmentDto>>> GetStudentSubjectAssignments(
            [FromQuery] int subjectAssignmentId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetStudentSubjectAssignments.Response response = await _mediator.Send(new GetStudentSubjectAssignments.Query(authorizedUser, subjectAssignmentId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<SubjectAssignmentDto>
                .ToIndexViewModel(response.SubjectAssignments, paginationFilter));
        }

        [HttpGet(nameof(GetStudentSubjectAssignmentSubmits))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> GetStudentSubjectAssignmentSubmits(
            [FromQuery] int subjectAssignmentId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetStudentSubjectAssignmentSubmits.Response response = await _mediator.Send(new GetStudentSubjectAssignmentSubmits.Query(authorizedUser, subjectAssignmentId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<SubjectAssignmentSubmitDto>
                .ToIndexViewModel(response.SubjectAssignments, paginationFilter));
        }
    }
}
