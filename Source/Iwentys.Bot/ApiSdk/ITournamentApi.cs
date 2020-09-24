using System.Threading.Tasks;
using Iwentys.Models.Transferable.Guilds;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface ITournamentApi
    {
        [Get("/api/Tournament/{id}")]
        Task<GuildProfileDto> Get(int id);
    }
}