using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using Iwentys.Endpoints.Api.Authorization;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.StudentProfile
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<StudentInfoDto>>> Get()
        {
            GetStudents.Response response = await _mediator.Send(new GetStudents.Query());
            return Ok(response.Students);
        }

        [HttpGet(nameof(GetSelf))]
        public async Task<ActionResult<StudentInfoDto>> GetSelf()
        {
            AuthorizedUser user = this.TryAuthWithIdentity(_userManager);
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