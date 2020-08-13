using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StudentFullProfileDto>> Get()
        {
            return Ok(_studentService.Get());
        }

        [HttpGet("{id}")]
        public ActionResult<StudentFullProfileDto> Get(int id)
        {
            return Ok(_studentService.Get(id));
        }

        [HttpPost]
        public ActionResult<StudentFullProfileDto> Get([FromBody] StudentUpdateDto studentUpdateDto)
        {
            AuthorizedUser user = AuthorizedUser.DebugAuth();

            return Ok(_studentService.AddGithubUsername(user.Id, studentUpdateDto.GithubUsername));
        }
    }
}