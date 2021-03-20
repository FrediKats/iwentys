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

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<AssignmentInfoDto>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<AssignmentInfoDto> assignments = await _assignmentService.GetStudentAssignment(user);
            return Ok(assignments);
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<AssignmentInfoDto>> Create([FromBody] AssignmentCreateArguments assignmentCreateArguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            var assignment = await _assignmentService.Create(user, assignmentCreateArguments);
            return Ok(assignment);
        }

        [HttpGet(nameof(Complete))]
        public async Task<ActionResult> Complete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.Complete(user, assignmentId);
            return Ok();
        }

        [HttpGet(nameof(Undo))]
        public async Task<ActionResult> Undo(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.Undo(user, assignmentId);
            return Ok();
        }

        [HttpGet(nameof(Delete))]
        public async Task<ActionResult> Delete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.Delete(user, assignmentId);
            return Ok();
        }
    }
}