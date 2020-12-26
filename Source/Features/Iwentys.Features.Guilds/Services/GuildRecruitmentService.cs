using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildRecruitmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<GuildEntity> _guildRepository;
        private readonly IGenericRepository<GuildRecruitmentMemberEntity> _guildRecruitmentMemberRepository;

        public GuildRecruitmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _guildRepository = _unitOfWork.GetRepository<GuildEntity>();
            _guildRecruitmentMemberRepository = _unitOfWork.GetRepository<GuildRecruitmentMemberEntity>();
        }

        public async Task<GuildRecruitmentEntity> Create(int guildId, int memberId, string description)
        {
            GuildEntity guild = await _guildRepository.FindByIdAsync(guildId);
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