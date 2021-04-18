using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildRecruitmentService
    {
        private readonly IGenericRepository<GuildRecruitment> _guildRecruitmentRepository;
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GuildRecruitmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRecruitmentRepository = _unitOfWork.GetRepository<GuildRecruitment>();
        }

        public async Task<GuildRecruitmentInfoDto> Create(int guildId, AuthorizedUser user, GuildRecruitmentCreateArguments createArguments)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser creator = await _iwentysUserRepository.GetById(user.Id);

            var guildRecruitment = GuildRecruitment.Create(creator, guild, createArguments);

            _guildRecruitmentRepository.Insert(guildRecruitment);
            await _unitOfWork.CommitAsync();

            return GuildRecruitmentInfoDto.FromEntity.Compile().Invoke(guildRecruitment);
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