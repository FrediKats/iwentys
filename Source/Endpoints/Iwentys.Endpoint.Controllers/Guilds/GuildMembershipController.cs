using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Models;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Guilds.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Guilds
{
    [Route("api/GuildMembership")]
    [ApiController]
    public class GuildMembershipController : ControllerBase
    {
        private readonly GuildMemberService _guildMemberService;

        public GuildMembershipController(GuildMemberService guildMemberService)
        {
            _guildMemberService = guildMemberService;
        }
        
        [HttpPut(nameof(Enter))]
        public async Task<ActionResult<GuildProfileDto>> Enter(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.EnterGuild(user, guildId));
        }

        [HttpPut(nameof(SendRequest))]
        public async Task<ActionResult<GuildProfileDto>> SendRequest(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.RequestGuild(user, guildId));
        }

        [HttpPut(nameof(Leave))]
        public async Task<ActionResult> Leave(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.LeaveGuild(user, guildId);
            return Ok();
        }

        [HttpGet(nameof(GetRequests))]
        public async Task<ActionResult<GuildMember[]>> GetRequests(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.GetGuildRequests(user, guildId));
        }

        [HttpGet(nameof(GetBlocked))]
        public async Task<ActionResult<GuildMember[]>> GetBlocked(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            return Ok(await _guildMemberService.GetGuildBlocked(user, guildId));
        }

        [HttpPut(nameof(BlockMember))]
        public async Task<IActionResult> BlockMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.BlockGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut(nameof(UnblockMember))]
        public async Task<IActionResult> UnblockMember(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.UnblockStudent(user, guildId, studentId);
            return Ok();
        }

        [HttpPut(nameof(KickMember))]
        public async Task<IActionResult> KickMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.KickGuildMember(user, guildId, memberId);
            return Ok();
        }

        [HttpPut(nameof(PromoteToMentor))]
        public async Task<IActionResult> PromoteToMentor(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.PromoteToMentor(user, memberId);
            return Ok();
        }

        [HttpGet(nameof(GetSelfMembership))]
        public async Task<ActionResult<UserMembershipState>> GetSelfMembership(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            UserMembershipState result = await _guildMemberService.GetUserMembership(user, guildId);
            return Ok(result);
        }

        [HttpPut(nameof(AcceptRequest))]
        public async Task<IActionResult> AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.AcceptRequest(user, guildId, studentId);
            return Ok();
        }

        [HttpPut(nameof(RejectRequest))]
        public async Task<IActionResult> RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _guildMemberService.RejectRequest(user, guildId, studentId);
            return Ok();
        }
    }
}