using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Repositories
{
    public interface IGuildRecruitmentRepository : IRepository<GuildRecruitmentEntity, int>
    {
        Task<GuildRecruitmentEntity> CreateAsync(GuildEntity guild, GuildMemberEntity creator, string description);
    }
}