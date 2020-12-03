using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Server.Source.Tools;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Services;
using Iwentys.Features.StudentFeature.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers.Study
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
        public async Task<ActionResult<List<StudentFullProfileDto>>> Get()
        {
            List<StudentFullProfileDto> students = await _studentService.GetAsync();
            return Ok(students);
        }

        [HttpGet("self")]
        public async Task<ActionResult<StudentFullProfileDto>> GetSelf()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentFullProfileDto result  = await _studentService.GetAsync(user.Id);
            return Ok(result);
        }


        [HttpGet("profile/{id}")]
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