using Iwentys.Database.Entities;
using Iwentys.Database.Repositories;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildProfileService
    {
        GuildProfile Create(int creator, GuildCreateArgumentDto arguments);
        GuildProfile Update(int creator, GuildUpdateArgumentDto arguments);

        GuildProfile[] Get();
        GuildProfile Get(int id);
    }
}