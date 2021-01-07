using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Recruitment;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildRecruitmentService
    {
        private readonly IGenericRepository<GuildRecruitmentMember> _guildRecruitmentMemberRepository;
        private readonly IGenericRepository<GuildRecruitment> _guildRecruitmentRepository;

        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GuildRecruitmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildRecruitmentRepository = _unitOfWork.GetRepository<GuildRecruitment>();
            _guildRecruitmentMemberRepository = _unitOfWork.GetRepository<GuildRecruitmentMember>();
        }

        public async Task<GuildRecruitment> Create(int guildId, AuthorizedUser user, GuildRecruitmentCreateArguments createArguments)
        {
            Guild guild = await _guildRepository.FindByIdAsync(guildId);
            GuildMember? creator = guild.Members.Find(m => m.MemberId == user.Id) ?? throw EntityNotFoundException.Create(typeof(GuildMember), user.Id);

            var guildRecruitment = GuildRecruitment.Create(creator.Member.EnsureIsGuildMentor(guild), guild, createArguments);

            await _guildRecruitmentRepository.InsertAsync(guildRecruitment);
            await _unitOfWork.CommitAsync();

            return guildRecruitment;
        }

        //TODO: implement closing recruitment
        //TODO: add edit opportunity
    }
}