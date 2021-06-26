using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Services
{
    public class GuildService
    {
        private readonly GithubIntegrationService _githubIntegrationService;

        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GuildService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
        }

        public async Task<GuildProfileShortInfoDto> Create(AuthorizedUser authorizedUser, GuildCreateRequestDto arguments)
        {
            IwentysUser creator = await _iwentysUserRepository.GetById(authorizedUser.Id);
            Guild userGuild = _guildMemberRepository.ReadForStudent(creator.Id);

            var guildEntity = Guild.Create(creator, userGuild, arguments);
            _guildRepository.Insert(guildEntity);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guildEntity);
        }

        public async Task<GuildProfileShortInfoDto> Update(AuthorizedUser user, GuildUpdateRequestDto arguments)
        {
            Guild guild = await _guildRepository.GetById(arguments.Id);
            IwentysUser iwentysUser = await _iwentysUserRepository.GetById(user.Id);

            guild.Update(iwentysUser, arguments);

            _guildRepository.Update(guild);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guild);
        }

        public async Task<GuildProfileDto> Get(int id)
        {
            return await _guildRepository
                .Get()
                .Where(g => g.Id == id)
                .Select(GuildProfileDto.FromEntity)
                .SingleAsync();
        }

        public async Task<GuildMemberLeaderBoardDto> GetGuildMemberLeaderBoard(int guildId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            return new GuildMemberLeaderBoardDto(guild.GetImpact());
        }

        public async Task UpdateGuildMemberImpact()
        {
            List<GuildMember> guildMembers = await _guildMemberRepository
                .Get()
                .Where(GuildMember.IsMember())
                .ToListAsync();

            foreach (GuildMember member in guildMembers)
            {
                ContributionFullInfo contributionFullInfo = await _githubIntegrationService.User.FindUserContributionOrEmpty(member.Member);
                member.MemberImpact = contributionFullInfo.Total;
            }

            _guildMemberRepository.Update(guildMembers);
            await _unitOfWork.CommitAsync();
        }

        public void UpdateGuildFromGithub(Guild guild)
        {
            OrganizationInfoDto organizationInfo = _githubIntegrationService.FindOrganizationInfo(guild.Title);
            if (organizationInfo is not null)
            {
                //TODO: need to fix after https://github.com/octokit/octokit.net/pull/2239

                guild.Bio = organizationInfo.Description;
                guild.ImageUrl = organizationInfo.ImageUrl;
                _guildRepository.Update(guild);
            }
        }
    }
}