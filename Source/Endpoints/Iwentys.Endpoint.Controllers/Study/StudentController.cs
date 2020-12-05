﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.StudentFeature.Domain;
using Iwentys.Features.StudentFeature.Models;
using Iwentys.Features.StudentFeature.Services;
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

        [HttpPut]
        public async Task<ActionResult<StudentFullProfileDto>> Update([FromBody] StudentUpdateRequest studentUpdateRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            StudentFullProfileDto student = await _studentService.AddGithubUsernameAsync(user.Id, studentUpdateRequest.GithubUsername);
            return Ok(student);
        }
    }
}