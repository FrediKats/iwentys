using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildProfileService
    {
        GuildProfileDto Create(int creatorId, GuildCreateArgumentDto arguments);
        GuildProfileDto Update(int creator, GuildUpdateArgumentDto arguments);
        GuildProfileDto ApproveGuildCreating(int adminId, int guildId);

        GuildProfileDto[] Get();
        GuildProfileDto Get(int id);
        GuildProfileDto GetUserProfile(int userId);
    }
}