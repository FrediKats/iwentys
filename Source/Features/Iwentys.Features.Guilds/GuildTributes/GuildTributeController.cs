using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.Guilds.Models;
using Iwentys.FeatureBase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Guilds.GuildTributes
{
    [Route("api/GuildTribute")]
    [ApiController]
    public class GuildTributeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildTributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<ActionResult<TributeInfoResponse>> Get(int tributeId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetGuildTributeById.Response response = await _mediator.Send(new GetGuildTributeById.Query(user, tributeId));
            return Ok(response.Tribute);
        }

        [HttpGet(nameof(GetPendingTributes))]
        public async Task<ActionResult<List<TributeInfoResponse>>> GetPendingTributes()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetPendingTributes.Response response = await _mediator.Send(new GetPendingTributes.Query(user));
            return Ok(response.Tributes);
        }

        [HttpGet(nameof(GetStudentTributeResult))]
        public async Task<ActionResult<List<TributeInfoResponse>>> GetStudentTributeResult()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetStudentTributeResult.Response response = await _mediator.Send(new GetStudentTributeResult.Query(user));
            return Ok(response.Tributes);
        }

        [HttpGet(nameof(GetByGuildId))]
        public async Task<ActionResult<List<TributeInfoResponse>>> GetByGuildId(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetGuildTributes.Response response = await _mediator.Send(new GetGuildTributes.Query(user, guildId));
            return Ok(response.Tribute);
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<TributeInfoResponse>> Create([FromBody] CreateProjectRequestDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CreateTribute.Response response = await _mediator.Send(new CreateTribute.Query(user, createProject));
            return Ok(response.Tribute);
        }

        [HttpPut(nameof(Cancel))]
        public async Task<ActionResult<TributeInfoResponse>> Cancel(long tributeId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CancelTribute.Response response = await _mediator.Send(new CancelTribute.Query(user, tributeId));
            return Ok(response.Tribute);
        }

        [HttpPut(nameof(Complete))]
        public async Task<ActionResult<TributeInfoResponse>> Complete([FromBody] TributeCompleteRequest tributeCompleteRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            CompleteTribute.Response response = await _mediator.Send(new CompleteTribute.Query(user, tributeCompleteRequest));
            return Ok(response.Tribute);
        }

        [HttpGet(nameof(FindStudentActiveTribute))]
        public async Task<ActionResult<TributeInfoResponse>> FindStudentActiveTribute()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            FindStudentActiveTribute.Response response = await _mediator.Send(new FindStudentActiveTribute.Query(user));
            TributeInfoResponse tributes = response.Tribute;
            if (tributes is null)
                return NotFound();
            return Ok(tributes);
        }
    }
}
