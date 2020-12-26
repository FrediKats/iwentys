using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<GuildEntity> _guildRepository;
        private readonly IGenericRepository<GuildMemberEntity> _guildMemberRepository;
        private readonly IGenericRepository<GuildPinnedProjectEntity> _guildPinnedProjectRepository;

        private readonly GithubIntegrationService _githubIntegrationService;

        public GuildService(GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _githubIntegrationService = githubIntegrationService;

            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _guildRepository = _unitOfWork.GetRepository<GuildEntity>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMemberEntity>();
            _guildPinnedProjectRepository = _unitOfWork.GetRepository<GuildPinnedProjectEntity>();
        }

        public async Task<GuildProfileShortInfoDto> CreateAsync(AuthorizedUser creator, GuildCreateRequestDto arguments)
        {
            StudentEntity creatorUser = await _studentRepository.FindByIdAsync(creator.Id);

            GuildEntity userGuild = _guildMemberRepository.ReadForStudent(creatorUser.Id);
            if (userGuild is not null)
                throw new InnerLogicException("Student already in guild");

            var guildEntity = GuildEntity.Create(creatorUser, arguments);
            await _guildRepository.InsertAsync(guildEntity);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(guildEntity);
        }

        public async Task<GuildProfileShortInfoDto> UpdateAsync(AuthorizedUser user, GuildUpdateRequestDto arguments)
        {
            StudentEntity student = await _studentRepository.FindByIdAsync(user.Id);
            GuildEntity info = await _guildRepository.FindByIdAsync(arguments.Id);
            student.EnsureIsGuildEditor(info);

            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.TestTaskLink = arguments.TestTaskLink ?? info.TestTaskLink;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMemberEntity guildMember in info.Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.MemberType = GuildMemberType.Member;

            _guildRepository.Update(info);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(info);
        }

        public async Task<GuildProfileShortInfoDto> ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _studentRepository.FindByIdAsync(user.Id);
            student.EnsureIsAdmin();

            GuildEntity guild = await _guildRepository.FindByIdAsync(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            _guildRepository.Update(guild);
            await _unitOfWork.CommitAsync();
            return new GuildProfileShortInfoDto(await _guildRepository.FindByIdAsync(guildId));
        }

        public List<GuildProfileDto> GetOverview(Int32 skippedCount, Int32 takenCount)
        {
            //TODO: add order
            return _guildRepository
                .Get()
                .Skip(skippedCount)
                .Take(takenCount)
                .Select(GuildProfileDto.FromEntity)
                .ToList();
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> GetAsync(int id, int? userId)
        {
            GuildEntity guild = await _guildRepository.FindByIdAsync(id);

            return await new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildMemberRepository)
                .ToExtendedGuildProfileDto(userId);
        }

        public GuildProfileDto FindStudentGuild(int userId)
        {
            GuildEntity guild = _guildMemberRepository.ReadForStudent(userId);
            if (guild is null)
                return null;

            return new GuildProfileDto(guild);
        }

        public async Task<GithubRepositoryInfoDto> AddPinnedRepositoryAsync(AuthorizedUser user, int guildId, string owner, string projectName)
        {
            GuildEntity guild = await _guildRepository.FindByIdAsync(guildId);
            StudentEntity profile = await _studentRepository.FindByIdAsync(user.Id);
            profile.EnsureIsGuildEditor(guild);

            GithubRepositoryInfoDto repositoryInfoDto = await _githubIntegrationService.GetRepository(owner, projectName);
            var guildPinnedProjectEntity = GuildPinnedProjectEntity.Create(guildId, repositoryInfoDto);
            await _guildPinnedProjectRepository.InsertAsync(guildPinnedProjectEntity);
            await _unitOfWork.CommitAsync();
            
            return repositoryInfoDto;
        }

        public async Task UnpinProject(AuthorizedUser user, int guildId, long pinnedProjectId)
        {
            GuildEntity guild = await _guildRepository.GetByIdAsync(guildId);
            StudentEntity profile = await _studentRepository.GetByIdAsync(user.Id);
            profile.EnsureIsGuildEditor(guild);

            var guildPinnedProjectEntity = await _guildPinnedProjectRepository.GetByIdAsync(pinnedProjectId);
            _guildPinnedProjectRepository.Delete(guildPinnedProjectEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<GuildMemberLeaderBoardDto> GetGuildMemberLeaderBoard(int guildId)
        {
            GuildEntity guild = await _guildRepository.FindByIdAsync(guildId);
            var domain = new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildMemberRepository);
            return await domain.GetMemberDashboard();
        }
    }
}