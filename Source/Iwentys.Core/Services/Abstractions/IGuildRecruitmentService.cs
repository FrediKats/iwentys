using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildRecruitmentService
    {
        GuildRecruitmentEntity Create(int guildId, int memberId, string description);
    }
}