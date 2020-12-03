using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Server.Source.Tools;
using Iwentys.Features.Assignments.Services;
using Iwentys.Features.Assignments.ViewModels;
using Iwentys.Features.StudentFeature;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentService _assignmentService;

        public AssignmentController(AssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssignmentInfoResponse>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<AssignmentInfoResponse> assignments = await _assignmentService.ReadAsync(user);
            return Ok(assignments);
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentInfoResponse>> Create([FromBody] AssignmentCreateRequest assignmentCreateRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            AssignmentInfoResponse assignment = await _assignmentService.CreateAsync(user, assignmentCreateRequest);
            return Ok(assignment);
        }

        //TODO: rework verbs
        [HttpGet("{assignmentId}/complete")]
        public async Task<ActionResult> Complete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.CompleteAsync(user, assignmentId);
            return Ok();
        }
    }
}