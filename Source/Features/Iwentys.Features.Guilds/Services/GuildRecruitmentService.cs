using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Recruitment;
using Microsoft.EntityFrameworkCore;

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

        public async Task<GuildRecruitmentInfoDto> Create(int guildId, AuthorizedUser user, GuildRecruitmentCreateArguments createArguments)
        {
            Guild guild = await _guildRepository.FindByIdAsync(guildId);
            GuildMember creator = guild.Members.Find(m => m.MemberId == user.Id) ?? throw EntityNotFoundException.Create(typeof(GuildMember), user.Id);

            var guildRecruitment = GuildRecruitment.Create(creator.Member.EnsureIsGuildMentor(guild), guild, createArguments);

            await _guildRecruitmentRepository.InsertAsync(guildRecruitment);
            await _unitOfWork.CommitAsync();

            return GuildRecruitmentInfoDto.FromEntity.Compile().Invoke(guildRecruitment);
        }

        public async Task CloseRecruitment(AuthorizedUser user, int guildId, int recruitmentId)
        {
            GuildRecruitment guildRecruitment = await _guildRecruitmentRepository.GetById(recruitmentId);
            Guild guild = await _guildRepository.FindByIdAsync(guildId);
            GuildMember mentor = guild.Members.Find(m => m.MemberId == user.Id) ?? throw EntityNotFoundException.Create(typeof(GuildMember), user.Id);

            guildRecruitment.Close(mentor.Member.EnsureIsGuildMentor(guild));

            _guildRecruitmentRepository.Update(guildRecruitment);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<GuildRecruitmentInfoDto>> GetGuildRecruitment(AuthorizedUser user, int guildId)
        {
            return await _guildRecruitmentRepository
                .Get()
                .Where(gr => gr.GuildId == guildId)
                .Select(GuildRecruitmentInfoDto.FromEntity)
                .ToListAsync();
        }
    }
}