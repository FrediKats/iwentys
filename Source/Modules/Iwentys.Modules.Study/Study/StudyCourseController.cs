using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Study.Study.Dtos;
using Iwentys.Modules.Study.Study.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Study.Study
{
    [Route("api/study-courses")]
    [ApiController]
    public class StudyCourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudyCourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudyCourseInfoDto>>> Get(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetStudyCourses.Response response = await _mediator.Send(new GetStudyCourses.Query());

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<StudyCourseInfoDto>
                .ToIndexViewModel(response.Courses, paginationFilter));
        }
    }
}