using System.Threading.Tasks;
using Iwentys.Infrastructure.Application.Modules.AccountManagment.MentorsManagment.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Modules.AccountManagment.MentorsManagment
{
    [Route("api/account-management/mentors")]
    [ApiController]
    public class MentorsManagmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MentorsManagmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            var subjectsMentors = await _mediator.Send(new GetAllSubjectsMentors.Query(authorizedUser));
            return Ok(subjectsMentors);
        }
    }
}