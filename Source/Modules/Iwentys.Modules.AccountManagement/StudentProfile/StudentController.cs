using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.AccountManagement.Dtos.Mentors;
using Iwentys.Modules.AccountManagement.StudentProfile.Dtos;
using Iwentys.Modules.AccountManagement.StudentProfile.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.AccountManagement.StudentProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<StudentInfoDto>>> Get(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetStudents.Response response = await _mediator.Send(new GetStudents.Query());
            
            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<StudentInfoDto>
                .ToIndexViewModel(response.Students, paginationFilter));
        }

        [HttpGet(nameof(GetSelf))]
        public async Task<ActionResult<StudentInfoDto>> GetSelf()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetStudentById.Response response = await _mediator.Send(new GetStudentById.Query(user.Id));
            return Ok(response.Student);
        }


        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<StudentInfoDto>> GetById(int id)
        {
            GetStudentById.Response response = await _mediator.Send(new GetStudentById.Query(id));
            return Ok(response.Student);
        }

        [HttpPut(nameof(UpdateProfile))]
        public async Task<ActionResult> UpdateProfile([FromBody] StudentUpdateRequestDto studentUpdateRequestDto)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            UpdateStudentProfile.Response response = await _mediator.Send(new UpdateStudentProfile.Query(authorizedUser, studentUpdateRequestDto));
            return Ok();
        }
    }
}