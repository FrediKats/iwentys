using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildService
    {
        private readonly GithubIntegrationService _githubIntegrationService;

        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<GuildPinnedProject> _guildPinnedProjectRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<GuildLastLeave> _guildLastLeaveRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GuildService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _guildPinnedProjectRepository = _unitOfWork.GetRepository<GuildPinnedProject>();
            _guildLastLeaveRepository = _unitOfWork.GetRepository<GuildLastLeave>();
        }

        public async Task<GuildProfileShortInfoDto> Create(AuthorizedUser authorizedUser, GuildCreateRequestDto arguments)
        {
            IwentysUser creator = await _iwentysUserRepository.GetById(authorizedUser.Id);

            Guild userGuild = _guildMemberRepository.ReadForStudent(creator.Id);
            if (userGuild is not null)
                throw new InnerLogicException("Student already in guild");

            var guildEntity = Guild.Create(creator, arguments);
            await _guildRepository.InsertAsync(guildEntity);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guildEntity);
        }

        public async Task<GuildProfileShortInfoDto> Update(AuthorizedUser user, GuildUpdateRequestDto arguments)
        {
            Guild guild = await _guildRepository.GetById(arguments.Id);
            GuildMentor guildMentor = await _iwentysUserRepository.GetById(user.Id).EnsureIsGuildMentor(guild);

            guild.Update(guildMentor, arguments);

            _guildRepository.Update(guild);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guild);
        }

        public async Task<GuildProfileShortInfoDto> ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            SystemAdminUser admin = await _iwentysUserRepository.GetById(user.Id).EnsureIsAdmin();
            Guild guild = await _guildRepository.GetById(guildId);

            guild.Approve(admin);

            _guildRepository.Update(guild);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(await _guildRepository.GetById(guildId));
        }

        public List<GuildProfileDto> GetOverview(int skippedCount, int takenCount)
        {
            return _guildRepository
                .Get()
                .Skip(skippedCount)
                .Take(takenCount)
                .Select(GuildProfileDto.FromEntity)
                .ToList();
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> Get(int id, int? userId)
        {
            Guild guild = await _guildRepository.GetById(id);

            return await new GuildDomain(guild, _githubIntegrationService, _iwentysUserRepository, _guildMemberRepository, _guildLastLeaveRepository)
                .ToExtendedGuildProfileDto(userId);
        }

        public GuildProfileDto FindStudentGuild(int userId)
        {
            Guild guild = _guildMemberRepository.ReadForStudent(userId);
            return guild.Maybe(g => new GuildProfileDto(g));
        }

        public async Task<GithubRepositoryInfoDto> AddPinnedRepository(AuthorizedUser user, int guildId, string owner, string projectName)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMentor guildMentor = await _iwentysUserRepository.GetById(user.Id).EnsureIsGuildMentor(guild);

            GithubRepositoryInfoDto repositoryInfoDto = await _githubIntegrationService.Repository.GetRepository(owner, projectName);
            var guildPinnedProjectEntity = GuildPinnedProject.Create(guildId, repositoryInfoDto);

            await _guildPinnedProjectRepository.InsertAsync(guildPinnedProjectEntity);
            await _unitOfWork.CommitAsync();
            return repositoryInfoDto;
        }

        public async Task UnpinProject(AuthorizedUser user, int guildId, long pinnedProjectId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMentor guildMentor = await _iwentysUserRepository.GetById(user.Id).EnsureIsGuildMentor(guild);

            GuildPinnedProject guildPinnedProjectEntity = await _guildPinnedProjectRepository.GetById(pinnedProjectId);

            _guildPinnedProjectRepository.Delete(guildPinnedProjectEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<GuildMemberLeaderBoardDto> GetGuildMemberLeaderBoard(int guildId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            var domain = new GuildDomain(guild, _githubIntegrationService, _iwentysUserRepository, _guildMemberRepository, _guildLastLeaveRepository);
            return await domain.GetMemberDashboard();
        }
    }
}