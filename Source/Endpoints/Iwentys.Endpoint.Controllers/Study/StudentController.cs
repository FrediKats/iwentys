using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Models.Students;
using Iwentys.Features.Study.Services;
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
        public async Task<ActionResult<List<StudentInfoDto>>> Get()
        {
            List<StudentInfoDto> students = await _studentService.Get();
            return Ok(students);
        }

        [HttpGet("self")]
        public async Task<ActionResult<StudentInfoDto>> GetSelf()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentInfoDto result  = await _studentService.Get(user.Id);
            return Ok(result);
        }


        [HttpGet("profile/{id}")]
        public async Task<ActionResult<StudentInfoDto>> Get(int id)
        {
            StudentInfoDto student = await _studentService.Get(id);
            return Ok(student);
        }

        [HttpPut]
        public async Task<ActionResult<StudentInfoDto>> Update([FromBody] StudentUpdateRequestDto studentUpdateRequestDto)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentInfoDto student = await _studentService.AddGithubUsername(user.Id, studentUpdateRequestDto.GithubUsername);
            return Ok(student);
        }
    }
}