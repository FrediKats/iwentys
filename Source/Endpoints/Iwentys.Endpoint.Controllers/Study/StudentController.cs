using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Students.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<List<StudentPartialProfileDto>>> Get()
        {
            List<StudentPartialProfileDto> students = await _studentService.GetAsync();
            return Ok(students);
        }

        [HttpGet("self")]
        public async Task<ActionResult<StudentPartialProfileDto>> GetSelf()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentPartialProfileDto result  = await _studentService.GetAsync(user.Id);
            return Ok(result);
        }


        [HttpGet("profile/{id}")]
        public async Task<ActionResult<StudentPartialProfileDto>> Get(int id)
        {
            StudentPartialProfileDto student = await _studentService.GetAsync(id);
            return Ok(student);
        }

        [HttpPut]
        public async Task<ActionResult<StudentPartialProfileDto>> Update([FromBody] StudentUpdateRequest studentUpdateRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentPartialProfileDto student = await _studentService.AddGithubUsernameAsync(user.Id, studentUpdateRequest.GithubUsername);
            return Ok(student);
        }
    }
}