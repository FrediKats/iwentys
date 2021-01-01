using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Features.Raids.Models;
using Iwentys.Features.Raids.Services;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/raids")]
    [ApiController]
    public class RaidController : ControllerBase
    {
        private RaidService _raidService;

        public RaidController(RaidService raidService)
        {
            _raidService = raidService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<List<RaidProfileDto>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<RaidProfileDto> result = await _raidService.Get(); 
            return Ok(result);
        }

        [HttpGet("profile/{raidId}")]
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

        [HttpPut("profile/{raidId}/unregister")]
        public async Task<ActionResult> UnRegisterOnRaid(int raidId)
        {
            AuthorizedUser user = this.TryAuthWithToken();
            await _raidService.UnRegisterOnRaid(user, raidId);
            return Ok();
        }
    }
}