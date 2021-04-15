using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Raids.Models;
using Iwentys.Features.Raids.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/raids")]
    [ApiController]
    public class RaidController : ControllerBase
    {
        private readonly RaidService _raidService;

        public RaidController(RaidService raidService)
        {
            _raidService = raidService;
        }

        [HttpPost("profile/create")]
        public async Task<ActionResult> Create([FromBody]RaidCreateArguments arguments)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _raidService.Create(user, arguments);
            return Ok();
        }

        [HttpGet("profile/get")]
        public async Task<ActionResult<List<RaidProfileDto>>> Get()
        {
            List<RaidProfileDto> result = await _raidService.Get(); 
            return Ok(result);
        }

        [HttpGet("profile/{raidId}/get")]
        public async Task<ActionResult<RaidProfileDto>> Get(int raidId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            RaidProfileDto result = await _raidService.Get(raidId);
            return Ok(result);
        }

        [HttpPut("profile/{raidId}/register")]
        public async Task<ActionResult> RegisterOnRaid(int raidId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _raidService.RegisterOnRaid(user, raidId);
            return Ok();
        }

        [HttpPut("profile/{raidId:int}/unregister")]
        public async Task<ActionResult> UnRegisterOnRaid(int raidId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _raidService.UnRegisterOnRaid(user, raidId);
            return Ok();
        }

        [HttpPut("profile/{raidId:int}/registration/{visitorId:int}/approve")]
        public async Task<ActionResult> ApproveRegistration(int raidId, int visitorId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _raidService.ApproveRegistration(user, raidId, visitorId);
            return Ok();
        }
    }
}