using System;
using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Students;
using Iwentys.Models.Types;
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
        public IEnumerable<StudentFullProfileDto> Get()
        {
            return _studentService.Get();
        }

        [HttpGet("{id}")]
        public StudentFullProfileDto Get(int id)
        {
            return _studentService.Get(id);
        }
    }
}