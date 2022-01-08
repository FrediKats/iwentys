using System.Threading.Tasks;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Guilds.GuildMemberships.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Guilds.GuildMemberships
{
    [Route("api/GuildMembership")]
    [ApiController]
    public class GuildMembershipController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildMembershipController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut(nameof(Enter))]
        public async Task<ActionResult> Enter(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            EnterGuild.Response response = await _mediator.Send(new EnterGuild.Query(user, guildId));
            return Ok();
        }

        [HttpPut(nameof(SendRequest))]
        public async Task<ActionResult> SendRequest(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            RequestGuildEnter.Response response = await _mediator.Send(new RequestGuildEnter.Query(user, guildId));
            return Ok();
        }

        [HttpPut(nameof(Leave))]
        public async Task<ActionResult> Leave(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            LeaveGuild.Response response = await _mediator.Send(new LeaveGuild.Query(user, guildId));
            return Ok();
        }

        [HttpGet(nameof(GetRequests))]
        public async Task<ActionResult<GuildMember[]>> GetRequests(
            [FromQuery] int guildId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetGuildRequests.Response response = await _mediator.Send(new GetGuildRequests.Query(user, guildId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<GuildMember>
                .ToIndexViewModel(response.GuildMembers, paginationFilter));
        }

        [HttpGet(nameof(GetBlocked))]
        public async Task<ActionResult<GuildMember[]>> GetBlocked(
            [FromQuery] int guildId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetGuildBlocked.Response response = await _mediator.Send(new GetGuildBlocked.Query(user, guildId));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<GuildMember>
                .ToIndexViewModel(response.GuildMembers, paginationFilter));
        }

        [HttpPut(nameof(BlockMember))]
        public async Task<IActionResult> BlockMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            BlockGuildMember.Response response = await _mediator.Send(new BlockGuildMember.Query(user, guildId, memberId));
            return Ok();
        }

        [HttpPut(nameof(UnblockMember))]
        public async Task<IActionResult> UnblockMember(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            UnblockStudent.Response response = await _mediator.Send(new UnblockStudent.Query(user, guildId, studentId));
            return Ok();
        }

        [HttpPut(nameof(KickMember))]
        public async Task<IActionResult> KickMember(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            KickGuildMember.Response response = await _mediator.Send(new KickGuildMember.Query(user, guildId, memberId));
            return Ok();
        }

        [HttpPut(nameof(PromoteToMentor))]
        public async Task<IActionResult> PromoteToMentor(int guildId, int memberId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            PromoteToMentor.Response response = await _mediator.Send(new PromoteToMentor.Query(user, guildId, memberId));
            return Ok();
        }

        [HttpGet(nameof(GetSelfMembership))]
        public async Task<ActionResult<UserMembershipState>> GetSelfMembership(int guildId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            GetUserMembership.Response response = await _mediator.Send(new GetUserMembership.Query(user, guildId));
            return Ok(response.Result);
        }

        [HttpPut(nameof(AcceptRequest))]
        public async Task<IActionResult> AcceptRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            AcceptGuildRequest.Response response = await _mediator.Send(new AcceptGuildRequest.Query(user, guildId, studentId));
            return Ok();
        }

        [HttpPut(nameof(RejectRequest))]
        public async Task<IActionResult> RejectRequest(int guildId, int studentId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            RejectGuildRequest.Response response = await _mediator.Send(new RejectGuildRequest.Query(user, guildId, studentId));
            return Ok();
        }
    }
}