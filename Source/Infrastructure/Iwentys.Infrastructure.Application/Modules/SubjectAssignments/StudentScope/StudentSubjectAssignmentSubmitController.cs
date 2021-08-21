﻿using System;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.Application.Modules.SubjectAssignments.Dtos;
using Iwentys.Infrastructure.Application.Modules.SubjectAssignments.StudentScope.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Modules.SubjectAssignments.StudentScope
{
    [Route("api/subject-assignment/student/Submit")]
    [ApiController]
    public class StudentSubjectAssignmentSubmitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentSubjectAssignmentSubmitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(CreateSubmit))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> CreateSubmit(SubjectAssignmentSubmitCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubmit.Response response = await _mediator.Send(new CreateSubmit.Query(authorizedUser, arguments));
            return Ok(response.Submit);
        }
    }
}