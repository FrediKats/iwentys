using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildProfileController : ControllerBase
    {
        private readonly IGuildProfileService _guildProfileService;

        public GuildProfileController(IGuildProfileService guildProfileService)
        {
            _guildProfileService = guildProfileService;
        }

        [HttpPost]
        public GuildProfileDto Create([FromQuery] int creatorId, [FromBody] GuildCreateArgumentDto arguments)
        {
            return _guildProfileService.Create(creatorId, arguments);
        }

        [HttpPost]
        public GuildProfileDto Update([FromQuery] int userId, [FromBody] GuildUpdateArgumentDto arguments)
        {
            return _guildProfileService.Update(userId, arguments);
        }

        [HttpGet]
        public IEnumerable<GuildProfileDto> Get()
        {
            return _guildProfileService.Get();
        }

        [HttpGet("{id}")]
        public GuildProfileDto Get(int id)
        {
            return _guildProfileService.Get(id);
        }
    }
}