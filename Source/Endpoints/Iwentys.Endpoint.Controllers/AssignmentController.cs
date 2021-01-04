using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Assignments.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentService _assignmentService;

        public AssignmentController(AssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssignmentInfoDto>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<AssignmentInfoDto> assignments = await _assignmentService.GetUserAssignments(user);
            return Ok(assignments);
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentInfoDto>> Create([FromBody] AssignmentCreateArguments assignmentCreateArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            var assignment = await _assignmentService.Create(user, assignmentCreateArguments);
            return Ok(assignment);
        }

        [HttpGet("{assignmentId}/complete")]
        public async Task<ActionResult> Complete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.Complete(user, assignmentId);
            return Ok();
        }

        [HttpGet("{assignmentId}/undo")]
        public async Task<ActionResult> Undo(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.Undo(user, assignmentId);
            return Ok();
        }

        [HttpGet("{assignmentId}/delete")]
        public async Task<ActionResult> Delete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.Delete(user, assignmentId);
            return Ok();
        }
    }
}