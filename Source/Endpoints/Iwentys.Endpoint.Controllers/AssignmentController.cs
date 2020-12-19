using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Assignments.Services;
using Iwentys.Features.Students.Domain;
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
            List<AssignmentInfoDto> assignments = await _assignmentService.ReadByUserAsync(user);
            return Ok(assignments);
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentInfoDto>> Create([FromBody] AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            var assignment = await _assignmentService.CreateAsync(user, assignmentCreateRequestDto);
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

        //TODO: rework verbs
        //TODO: it isn't work =\
        [HttpGet("{assignmentId}/delete")]
        public async Task<ActionResult> Delete(int assignmentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _assignmentService.DeleteAsync(user, assignmentId);
            return Ok();
        }
    }
}