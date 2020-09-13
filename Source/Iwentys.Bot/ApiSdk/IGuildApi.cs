using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IGuildApi
    {
        [Get("/api/Guild/")]
        public Task<List<GuildProfilePreviewDto>> GetOverview(int? skip = 0, int? take = 20);

        [Get("/api/Guild/{id}")]
        Task<GuildProfileDto> Get(int id);

        [Put("/api/Guild/{guildId}/enter")]
        public Task<GuildProfileDto> Enter(int guildId);

        [Put("/api/Guild/{guildId}/request")]
        public Task<GuildProfileDto> SendRequest(int guildId);

        [Put("/api/Guild/{guildId}/leave")]
        public Task<GuildProfileDto> Leave(int guildId);

        [Get("/api/Guild/{guildId}/request")]
        public Task<List<GuildMemberEntity>> GetGuildRequests(int guildId);
    }
}