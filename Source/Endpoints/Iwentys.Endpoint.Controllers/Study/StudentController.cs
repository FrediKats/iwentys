using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.AccountManagement.Services;
using Iwentys.Features.Study.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Study
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly IwentysUserService _userService;

        public StudentController(StudentService studentService, IwentysUserService userService)
        {
            _studentService = studentService;
            _userService = userService;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<StudentInfoDto>>> Get()
        {
            List<StudentInfoDto> students = await _studentService.Get();
            return Ok(students);
        }

        [HttpGet(nameof(GetSelf))]
        public async Task<ActionResult<StudentInfoDto>> GetSelf()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentInfoDto result  = await _studentService.Get(user.Id);
            return Ok(result);
        }


        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<StudentInfoDto>> GetById(int id)
        {
            StudentInfoDto student = await _studentService.Get(id);
            return Ok(student);
        }

        [HttpPut(nameof(UpdateProfile))]
        public async Task<ActionResult<IwentysUserInfoDto>> UpdateProfile([FromBody] StudentUpdateRequestDto studentUpdateRequestDto)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            IwentysUserInfoDto result = await _userService.AddGithubUsername(authorizedUser.Id, studentUpdateRequestDto.GithubUsername);
            return Ok(result);
        }
    }
}