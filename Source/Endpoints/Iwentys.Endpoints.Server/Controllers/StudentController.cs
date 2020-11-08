using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoints.OldServer.Tools;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Services;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.OldServer.Controllers
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

        [HttpGet]
        public async Task<ActionResult<List<StudentFullProfileDto>>> Get()
        {
            List<StudentFullProfileDto> students = await _studentService.GetAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentFullProfileDto>> Get(int id)
        {
            StudentFullProfileDto student = await _studentService.GetAsync(id);
            return Ok(student);
        }

        [HttpGet("for-group/{groupName}")]
        public async Task<ActionResult<List<StudentFullProfileDto>>> Get(string groupName)
        {
            List<StudentFullProfileDto> students = await _studentService.GetAsync(groupName);
            return Ok(students);
        }

        [HttpPut]
        public async Task<ActionResult<StudentFullProfileDto>> Update([FromBody] StudentUpdateRequest studentUpdateRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentFullProfileDto student = await _studentService.AddGithubUsernameAsync(user.Id, studentUpdateRequest.GithubUsername);
            return Ok(student);
        }
    }
}