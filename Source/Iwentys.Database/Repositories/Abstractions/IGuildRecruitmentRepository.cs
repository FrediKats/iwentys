using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildRecruitmentRepository : IGenericRepository<GuildRecruitmentEntity, int>
    {
        GuildRecruitmentEntity Create(GuildEntity guild, GuildMemberEntity creator, string description);
    }
}