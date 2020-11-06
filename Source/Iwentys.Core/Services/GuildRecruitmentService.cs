using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Core.Services
{
    public class GuildRecruitmentService
    {
        private readonly DatabaseAccessor _databaseAccessor;

        public GuildRecruitmentService(DatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public async Task<GuildRecruitmentEntity> Create(int guildId, int memberId, string description)
        {
            GuildEntity guild = await _databaseAccessor.Guild.GetAsync(guildId);
            return await _databaseAccessor.GuildRecruitment.CreateAsync(guild, guild.Members.Find(m => m.MemberId == memberId), description);
        }
    }
}