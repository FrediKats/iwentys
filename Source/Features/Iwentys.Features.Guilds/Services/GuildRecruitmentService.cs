using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildRecruitmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<GuildRecruitmentMember> _guildRecruitmentMemberRepository;

        public GuildRecruitmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildRecruitmentMemberRepository = _unitOfWork.GetRepository<GuildRecruitmentMember>();
        }

        public async Task<GuildRecruitment> Create(int guildId, int memberId, string description)
        {
            Guild guild = await _guildRepository.FindByIdAsync(guildId);
            var creator = guild.Members.Find(m => m.MemberId == memberId) ?? throw EntityNotFoundException.Create(typeof(GuildMember), memberId);

            GuildRecruitment recruitment = new GuildRecruitment
            {
                Description = description,
                GuildId = guild.Id
            };

            var guildRecruitmentMemberEntity = new GuildRecruitmentMember
            {
                GuildRecruitment = recruitment,
                MemberId = creator.MemberId
            };

            await _guildRecruitmentMemberRepository.InsertAsync(guildRecruitmentMemberEntity);
            await _unitOfWork.CommitAsync();
            return recruitment;
        }

        //TODO: implement closing recruitment
        //TODO: add more info about recruitment
        //TODO: add edit opportunity
    }
}