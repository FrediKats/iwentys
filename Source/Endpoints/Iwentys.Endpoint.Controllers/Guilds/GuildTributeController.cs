using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using Iwentys.Features.Guilds.Tributes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/GuildTribute")]
    [ApiController]
    public class GuildTributeController : ControllerBase
    {
        private readonly GuildTributeService _guildService;

        public GuildTributeController(GuildTributeService guildService)
        {
            _guildService = guildService;
        }

        [HttpGet()]
        public ActionResult<TributeInfoResponse> Get(int tributeId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            Task<TributeInfoResponse> result = _guildService.Get(user, tributeId);
            return Ok(result);
        }

        [HttpGet(nameof(GetPendingTributes))]
        public ActionResult<List<TributeInfoResponse>> GetPendingTributes()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.GetPendingTributes(user));
        }

        [HttpGet(nameof(GetStudentTributeResult))]
        public ActionResult<List<TributeInfoResponse>> GetStudentTributeResult()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(_guildService.GetStudentTributeResult(user));
        }

        [HttpGet(nameof(GetByGuildId))]
        public ActionResult<List<TributeInfoResponse>> GetByGuildId(int guildId)
        {
            return Ok(_guildService.GetGuildTributes(guildId));
        }

        [HttpPost(nameof(Create))]
        public async Task<ActionResult<TributeInfoResponse>> Create([FromBody] CreateProjectRequestDto createProject)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.CreateTribute(user, createProject);
            return Ok(tributes);
        }

        [HttpPut(nameof(Cancel))]
        public async Task<ActionResult<TributeInfoResponse>> Cancel(long tributeId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.CancelTribute(user, tributeId);
            return Ok(tributes);
        }

        [HttpPut(nameof(Complete))]
        public async Task<ActionResult<TributeInfoResponse>> Complete([FromBody] TributeCompleteRequest tributeCompleteRequest)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.CompleteTribute(user, tributeCompleteRequest);
            return Ok(tributes);
        }

        [HttpGet(nameof(FindStudentActiveTribute))]
        public async Task<ActionResult<TributeInfoResponse>> FindStudentActiveTribute()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            TributeInfoResponse tributes = await _guildService.FindStudentActiveTribute(user);
            if (tributes is null)
                return NotFound();
            return Ok(tributes);
        }
    }
}
