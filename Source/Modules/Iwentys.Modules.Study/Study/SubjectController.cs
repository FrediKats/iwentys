using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Study.Study.Dtos;
using Iwentys.Modules.Study.Study.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Study.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(SearchSubjects))]
        public async Task<ActionResult<List<SubjectProfileDto>>> SearchSubjects(
            [FromQuery] int? courseId, 
            [FromQuery] StudySemester? semester,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            var studySearchParameters = new StudySearchParametersDto(null, null, courseId, semester, 0, 20);
            SearchSubjects.Response response = await _mediator.Send(new SearchSubjects.Query(studySearchParameters));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<SubjectProfileDto>
                .ToIndexViewModel(response.Subjects, paginationFilter));
        }

        [HttpGet(nameof(GetSubjectById))]
        public async Task<ActionResult<SubjectProfileDto>> GetSubjectById(int subjectId)
        {
            GetSubjectById.Response response = await _mediator.Send(new GetSubjectById.Query(subjectId));
            return Ok(response.Subject);
        }

        [HttpGet(nameof(GetSubjectsByGroupId))]
        public async Task<ActionResult<List<SubjectProfileDto>>> GetSubjectsByGroupId(
            [FromQuery] int groupId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetSubjectsByGroupId.Response response = await _mediator.Send(new GetSubjectsByGroupId.Query(groupId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<SubjectProfileDto>
                .ToIndexViewModel(response.Subjects, paginationFilter));
        }
    }
}