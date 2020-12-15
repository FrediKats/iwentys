using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildRecruitmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<GuildRecruitmentMemberEntity> _guildRecruitmentMemberRepository;

        private readonly IGuildRepository _guildRepository;

        public GuildRecruitmentService(IGuildRepository guildRepository, IUnitOfWork unitOfWork)
        {
            _guildRepository = guildRepository;
            _unitOfWork = unitOfWork;
            _guildRecruitmentMemberRepository = _unitOfWork.GetRepository<GuildRecruitmentMemberEntity>();
        }

        public async Task<GuildRecruitmentEntity> Create(int guildId, int memberId, string description)
        {
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            var creator = guild.Members.Find(m => m.MemberId == memberId) ?? throw EntityNotFoundException.Create(typeof(GuildMemberEntity), memberId);

            GuildRecruitmentEntity recruitment = new GuildRecruitmentEntity
            {
                Description = description,
                GuildId = guild.Id
            };

            var guildRecruitmentMemberEntity = new GuildRecruitmentMemberEntity
            {
                GuildRecruitment = recruitment,
                MemberId = creator.MemberId
            };

            await _guildRecruitmentMemberRepository.InsertAsync(guildRecruitmentMemberEntity);
            await _unitOfWork.CommitAsync();
            return recruitment;
        }
    }
}