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
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildService
    {
        private readonly GuildRepositoriesScope _database;
        private readonly GithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _githubApiAccessor;

        public GuildService(GuildRepositoriesScope database, GithubUserDataService githubUserDataService, IGithubApiAccessor githubApiAccessor)
        {
            _database = database;
            _githubUserDataService = githubUserDataService;
            _githubApiAccessor = githubApiAccessor;
        }

        public async Task<GuildProfileShortInfoDto> CreateAsync(AuthorizedUser creator, GuildCreateRequest arguments)
        {
            StudentEntity creatorUser = await _database.Student.GetAsync(creator.Id);

            GuildEntity userGuild = _database.Guild.ReadForStudent(creatorUser.Id);
            if (userGuild != null)
                throw new InnerLogicException("Student already in guild");

            return _database.Guild.Create(creatorUser, arguments)
                .To(g => new GuildDomain(g, _githubUserDataService, _database))
                .ToGuildProfileShortInfoDto();
        }

        public async Task<GuildProfileShortInfoDto> UpdateAsync(AuthorizedUser user, GuildUpdateRequest arguments)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            GuildEntity info = await _database.Guild.GetAsync(arguments.Id);
            student.EnsureIsGuildEditor(info);

            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.TestTaskLink = arguments.TestTaskLink ?? info.TestTaskLink;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMemberEntity guildMember in info.Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.MemberType = GuildMemberType.Member;

            GuildEntity updatedGuid = await _database.Guild.UpdateAsync(info);
            return new GuildDomain(updatedGuid, _githubUserDataService, _database).ToGuildProfileShortInfoDto();
        }

        public async Task<GuildProfileShortInfoDto> ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            StudentEntity student = await _database.Student.GetAsync(user.Id);
            student.EnsureIsAdmin();

            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            GuildEntity updatedGuid = await _database.Guild.UpdateAsync(guild);
            return new GuildDomain(updatedGuid, _githubUserDataService, _database).ToGuildProfileShortInfoDto();
        }

        public GuildProfileDto[] Get()
        {
            return _database.Guild.Read().AsEnumerable().Select(g =>
                new GuildDomain(g, _githubUserDataService, _database)
                    .ToGuildProfileDto().Result).ToArray();
        }

        public GuildProfilePreviewDto[] GetOverview(Int32 skippedCount, Int32 takenCount)
        {
            return _database.Guild.Read()
                .ToList()
                .Select(g => new GuildDomain(g, _githubUserDataService, _database).ToGuildProfilePreviewDto())
                .OrderByDescending(g => g.Rating)
                .Skip(skippedCount)
                .Take(takenCount)
                .ToArray();
        }

        public async Task<GuildProfileDto> GetAsync(int id, int? userId)
        {
            GuildEntity guild = await _database.Guild.GetAsync(id);

            return await new GuildDomain(guild, _githubUserDataService, _database)
                .ToGuildProfileDto(userId);
        }

        public async Task<GuildProfileDto> FindStudentGuild(int userId)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(userId);
            if (guild is null)
                return null;
            return await guild
                .To(g => new GuildDomain(g, _githubUserDataService, _database))
                .ToGuildProfileDto(userId);
        }

        public async Task<GithubRepository> AddPinnedRepositoryAsync(AuthorizedUser user, int guildId, string owner, string projectName)
        {
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            StudentEntity profile = await user.GetProfile(_database.Student);
            profile.EnsureIsGuildEditor(guild);

            //TODO: add work with cache
            GithubRepository repository = _githubApiAccessor.GetRepository(owner, projectName);
            await _database.Guild.PinProjectAsync(guildId, repository);
            return repository;
        }

        public async Task UnpinProject(AuthorizedUser user, int guildId, long pinnedProjectId)
        {
            GuildEntity guild = await _database.Guild.ReadByIdAsync(guildId);
            StudentEntity profile = await user.GetProfile(_database.Student);
            profile.EnsureIsGuildEditor(guild);

            _database.Guild.UnpinProject(pinnedProjectId);
        }

        public async Task<GuildMemberLeaderBoard> GetGuildMemberLeaderBoard(int guildId)
        {
            GuildEntity guild = await _database.Guild.GetAsync(guildId);
            return new GuildDomain(guild, _githubUserDataService, _database).GetMemberDashboard();
        }
    }
}