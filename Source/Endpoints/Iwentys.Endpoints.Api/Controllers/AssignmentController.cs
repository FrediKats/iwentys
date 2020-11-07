using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Core.Services;
using Iwentys.Endpoints.Api.Tools;
using Iwentys.Features.StudentFeature;
using Iwentys.Models.Transferable;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.Api.Controllers
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
        public async Task<ActionResult<List<AssignmentInfoResponse>>> Create([FromBody] AssignmentCreateRequest assignmentCreateRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            AssignmentInfoResponse assignment = await _assignmentService.CreateAsync(user, assignmentCreateRequest);
            return Ok(assignment);
        }
    }
}