using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/guild")]
    [ApiController]
    public class GuildMemberController : ControllerBase
    {
        private readonly GuildMemberService _guildMemberService;

        public GuildMemberController(GuildMemberService guildMemberService)
        {
            _guildMemberService = guildMemberService;
        }
        
        [HttpPut("{guildId}/enter")]
        public async Task<ActionResult<GuildProfileDto>> Enter(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.EnterGuildAsync(user, guildId));
        }

        [HttpPut("{guildId}/request")]
        public async Task<ActionResult<GuildProfileDto>> SendRequest(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.RequestGuildAsync(user, guildId));
        }

        //TODO: leave can be without guild id
        [HttpPut("{guildId}/leave")]
        public async Task<ActionResult> Leave(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.LeaveGuildAsync(user, guildId);
            return Ok();
        }

        [HttpGet("{guildId}/request")]
        public async Task<ActionResult<GuildMemberEntity[]>> GetGuildRequests(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.GetGuildRequests(user, guildId));
        }

        [HttpGet("{guildId}/blocked")]
        public async Task<ActionResult<GuildMemberEntity[]>> GetGuildBlocked(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.GetGuildBlocked(user, guildId));
        }

        [HttpPut("{guildId}/member/{memberId}/block")]
        public async Task<IActionResult> BlockGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.BlockGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/blocked/{studentId}/unblock")]
        public async Task<IActionResult> UnblockGuildMember(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.UnblockStudent(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/member/{memberId}/kick")]
        public async Task<IActionResult> KickGuildMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.KickGuildMemberAsync(user, guildId, memberId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/accept")]
        public async Task<IActionResult> AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.AcceptRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPut("{guildId}/request/{studentId}/reject")]
        public async Task<IActionResult> RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.RejectRequest(user, guildId, studentId);
            return Ok();
        }
    }
}