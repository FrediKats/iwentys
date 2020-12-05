using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildService
    {
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IGithubApiAccessor _githubApiAccessor;
        private readonly IStudentRepository _studentRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;

        public GuildService(GithubIntegrationService githubIntegrationService, IGithubApiAccessor githubApiAccessor, IStudentRepository studentRepository, IGuildRepository guildRepository, IGuildMemberRepository guildMemberRepository)
        {
            _githubIntegrationService = githubIntegrationService;
            _githubApiAccessor = githubApiAccessor;
            _studentRepository = studentRepository;
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
        }

        public async Task<GuildProfileShortInfoDto> CreateAsync(AuthorizedUser creator, GuildCreateRequestDto arguments)
        {
            StudentEntity creatorUser = await _studentRepository.GetAsync(creator.Id);

            GuildEntity userGuild = _guildRepository.ReadForStudent(creatorUser.Id);
            if (userGuild != null)
                throw new InnerLogicException("Student already in guild");
            
            GuildEntity createdGuild = _guildRepository.Create(creatorUser, arguments);
            return new GuildProfileShortInfoDto(createdGuild);
        }

        public async Task<GuildProfileShortInfoDto> UpdateAsync(AuthorizedUser user, GuildUpdateRequestDto arguments)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            GuildEntity info = await _guildRepository.GetAsync(arguments.Id);
            student.EnsureIsGuildEditor(info);

            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.TestTaskLink = arguments.TestTaskLink ?? info.TestTaskLink;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMemberEntity guildMember in info.Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.MemberType = GuildMemberType.Member;

            GuildEntity updatedGuid = await _guildRepository.UpdateAsync(info);
            return new GuildProfileShortInfoDto(updatedGuid);
        }

        public async Task<GuildProfileShortInfoDto> ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _studentRepository.GetAsync(user.Id);
            student.EnsureIsAdmin();

            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            await _guildRepository.UpdateAsync(guild);
            return new GuildProfileShortInfoDto(await _guildRepository.GetAsync(guildId));
        }

        public GuildProfileShortInfoWithLeaderDto[] GetOverview(Int32 skippedCount, Int32 takenCount)
        {
            return _guildRepository.Read()
                .ToList()
                .Select(g => new GuildProfileShortInfoWithLeaderDto(g))
                .Skip(skippedCount)
                .Take(takenCount)
                .ToArray();
        }

        public async Task<ExtendedGuildProfileWithMemberDataDto> GetAsync(int id, int? userId)
        {
            GuildEntity guild = await _guildRepository.GetAsync(id);

            return await new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildRepository, _guildMemberRepository)
                .ToExtendedGuildProfileDto(userId);
        }

        public GuildProfileDto FindStudentGuild(int userId)
        {
            GuildEntity guild = _guildRepository.ReadForStudent(userId);
            if (guild is null)
                return null;

            return new GuildProfileDto(guild);
        }

        public async Task<GithubRepositoryInfoDto> AddPinnedRepositoryAsync(AuthorizedUser user, int guildId, string owner, string projectName)
        {
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            StudentEntity profile = await user.GetProfile(_studentRepository);
            profile.EnsureIsGuildEditor(guild);

            //TODO: add work with cache
            GithubRepositoryInfoDto repositoryInfoDto = _githubApiAccessor.GetRepository(owner, projectName);
            await _guildRepository.PinProjectAsync(guildId, repositoryInfoDto);
            return repositoryInfoDto;
        }

        public async Task UnpinProject(AuthorizedUser user, int guildId, long pinnedProjectId)
        {
            GuildEntity guild = await _guildRepository.ReadByIdAsync(guildId);
            StudentEntity profile = await user.GetProfile(_studentRepository);
            profile.EnsureIsGuildEditor(guild);

            _guildRepository.UnpinProject(pinnedProjectId);
        }

        public async Task<GuildMemberLeaderBoardDto> GetGuildMemberLeaderBoard(int guildId)
        {
            GuildEntity guild = await _guildRepository.GetAsync(guildId);
            return new GuildDomain(guild, _githubIntegrationService, _studentRepository, _guildRepository, _guildMemberRepository).GetMemberDashboard();
        }
    }
}