using System.Collections.Generic;
using Iwentys.Api.Tools;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Implementations;
using Iwentys.Models.Transferable;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
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
        public ActionResult<List<AssignmentInfoDto>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_assignmentService.Read(user));
        }

        [HttpPost]
        public ActionResult<List<AssignmentInfoDto>> Create([FromBody] AssignmentCreateDto assignmentCreateDto)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_assignmentService.Create(user, assignmentCreateDto));
        }
    }
}