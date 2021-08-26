using System.Threading.Tasks;
using Iwentys.Infrastructure.Application;
using Iwentys.Modules.AccountManagement.Mentors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.AccountManagement.Mentors
{
    [Route("api/account-management/mentors")]
    [ApiController]
    public class MentorsManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MentorsManagementController(IMediator mediator)
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