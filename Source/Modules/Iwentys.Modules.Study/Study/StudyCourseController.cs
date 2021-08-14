﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application.Controllers.Study;
using Iwentys.Infrastructure.Application.Controllers.Study.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Study.Study
{
    [Route("api/study-courses")]
    [ApiController]
    public class StudyCourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudyCourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudyCourseInfoDto>>> Get()
        {
            GetStudyCourses.Response response = await _mediator.Send(new GetStudyCourses.Query());
            return Ok(response.Courses);
        }
    }
}