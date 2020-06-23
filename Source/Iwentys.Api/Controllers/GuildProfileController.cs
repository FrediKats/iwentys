using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Entities;
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

        [HttpGet]
        public GuildProfile Get(int creatorId, [FromBody] GuildCreateArgumentDto arguments)
        {
            return _guildProfileService.Create(creatorId, arguments);
        }

        [HttpGet]
        public GuildProfile Update(int userId, [FromBody] GuildUpdateArgumentDto arguments)
        {
            return _guildProfileService.Update(userId, arguments);
        }

        [HttpGet]
        public IEnumerable<GuildProfile> Get()
        {
            return _guildProfileService.Get();
        }

        [HttpGet("{id}")]
        public GuildProfile Get(int id)
        {
            return _guildProfileService.Get(id);
        }
    }
}
