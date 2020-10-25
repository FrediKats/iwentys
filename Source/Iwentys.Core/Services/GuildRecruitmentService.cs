using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
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

        public GuildRecruitmentEntity Create(int guildId, int memberId, string description)
        {
            GuildEntity guild = _databaseAccessor.Guild.Get(guildId);
            return _databaseAccessor.GuildRecruitment.Create(guild, guild.Members.Find(m => m.MemberId == memberId), description);
        }
    }
}