using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildRecruitmentService
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildRecruitmentRepository _guildRecruitmentRepository;


        public GuildRecruitmentService(IGuildRepository guildRepository, IGuildRecruitmentRepository guildRecruitmentRepository)
        {
            _guildRepository = guildRepository;
            _guildRecruitmentRepository = guildRecruitmentRepository;
        }

        public async Task<GuildRecruitmentEntity> Create(int guildId, int memberId, string description)
        {
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            return await _guildRecruitmentRepository.CreateAsync(guild, guild.Members.Find(m => m.MemberId == memberId), description);
        }
    }
}