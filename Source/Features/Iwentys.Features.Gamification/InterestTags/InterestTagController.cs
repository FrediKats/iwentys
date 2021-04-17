using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Gamification.InterestTags
{
    [Route("api/InterestTag")]
    [ApiController]
    public class InterestTagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterestTagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<InterestTagDto>>> Get()
        {
            GetAllTags.Response response = await _mediator.Send(new GetAllTags.Query());
            return Ok(response.Tags);
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<InterestTagDto>>> GetByStudentId(int studentId)
        {
            GetUserTags.Response response = await _mediator.Send(new GetUserTags.Query(studentId));
            return Ok(response.Tags);
        }

        [HttpGet(nameof(Add))]
        public async Task<ActionResult> Add(int studentId, int tagId)
        {
            AddUserTag.Response response = await _mediator.Send(new AddUserTag.Query(studentId, tagId));
            return Ok();
        }
        
        [HttpGet(nameof(Remove))]
        public async Task<ActionResult> Remove(int studentId, int tagId)
        {
            RemoveUserTag.Response response = await _mediator.Send(new RemoveUserTag.Query(studentId, tagId));
            return Ok();
        }
    }
}