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

        [HttpGet]
        public async Task<ActionResult<List<RaidProfileDto>>> Get()
        {
            AuthorizedUser user = this.TryAuthWithToken();
            List<RaidProfileDto> result = await _raidService.Get(); 
            return Ok(result);
        }
    }
}