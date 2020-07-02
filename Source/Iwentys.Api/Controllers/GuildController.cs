using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly IGuildService _guildService;

        public GuildController(IGuildService guildService)
        {
            _guildService = guildService;
        }

        [HttpPost]
        public GuildProfileDto Create([FromQuery] int creatorId, [FromBody] GuildCreateArgumentDto arguments)
        {
            return _guildService.Create(creatorId, arguments);
        }

        [HttpPost]
        public GuildProfileDto Update([FromQuery] int userId, [FromBody] GuildUpdateArgumentDto arguments)
        {
            return _guildService.Update(userId, arguments);
        }

        [HttpGet]
        public IEnumerable<GuildProfileDto> Get()
        {
            return _guildService.Get();
        }

        [HttpGet("{id}")]
        public GuildProfileDto Get(int id)
        {
            return _guildService.Get(id);
        }
    }
}