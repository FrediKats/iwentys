using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    [Route("api/SubjectAssignmentSubmit")]
    [ApiController]
    public class SubjectAssignmentSubmitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectAssignmentSubmitController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(SearchSubjectAssignmentSubmits))]
        public async Task<ActionResult<List<SubjectAssignmentSubmitDto>>> SearchSubjectAssignmentSubmits(SubjectAssignmentSubmitSearchArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SearchSubjectAssignmentSubmits.Response response = await _mediator.Send(new SearchSubjectAssignmentSubmits.Query(arguments, authorizedUser));
            return Ok(response.Submits);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> GetById(int subjectAssignmentSubmitId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            GetSubjectAssignmentSubmit.Response response = await _mediator.Send(new GetSubjectAssignmentSubmit.Query(subjectAssignmentSubmitId, authorizedUser));
            return Ok(response.Submit);
        }

        [HttpPost(nameof(CreateSubmit))]
        public async Task<ActionResult<SubjectAssignmentSubmitDto>> CreateSubmit(SubjectAssignmentSubmitCreateArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            CreateSubmit.Response response = await _mediator.Send(new CreateSubmit.Query(authorizedUser, arguments));
            return Ok(response.Submit);
        }

        [HttpPut(nameof(SendSubmitFeedback))]
        public async Task<ActionResult> SendSubmitFeedback(SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            SendSubmitFeedback.Response response = await _mediator.Send(new SendSubmitFeedback.Query(authorizedUser, arguments));
            return Ok();
        }
    }
}