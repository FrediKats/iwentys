using System;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.DomainModel.Guilds;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildService : IGuildService
    {
        private readonly DatabaseAccessor _database;
        private readonly IGithubUserDataService _githubUserDataService;
        private readonly IGithubApiAccessor _githubApiAccessor;

        public GuildService(DatabaseAccessor database, IGithubUserDataService githubUserDataService, IGithubApiAccessor githubApiAccessor)
        {
            _database = database;
            _githubUserDataService = githubUserDataService;
            _githubApiAccessor = githubApiAccessor;
        }

        public GuildProfileShortInfoDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments)
        {
            StudentEntity creatorUser = _database.Student.Get(creator.Id);

            GuildEntity userGuild = _database.Guild.ReadForStudent(creatorUser.Id);
            if (userGuild != null)
                throw new InnerLogicException("Student already in guild");

            return _database.Guild.Create(creatorUser, arguments)
                .To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .ToGuildProfileShortInfoDto();
        }

        public GuildProfileShortInfoDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            GuildEntity info = _database.Guild.Get(arguments.Id);
            student.EnsureIsGuildEditor(info);

            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.TestTaskLink = arguments.TestTaskLink ?? info.TestTaskLink;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (GuildMemberEntity guildMember in info.Members.Where(guildMember => guildMember.MemberType == GuildMemberType.Requested))
                    guildMember.MemberType = GuildMemberType.Member;

            return _database.Guild.Update(info)
                .To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .ToGuildProfileShortInfoDto();
        }

        public GuildProfileShortInfoDto ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            _database.Student
                .Get(user.Id)
                .EnsureIsAdmin();

            GuildEntity guild = _database.Guild.Get(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            return _database.Guild.Update(guild)
                .To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .ToGuildProfileShortInfoDto();
        }

        public GuildProfileDto[] Get()
        {
            return _database.Guild.Read().AsEnumerable().Select(g =>
                new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor)
                    .ToGuildProfileDto()).ToArray();
        }

        public GuildProfilePreviewDto[] GetOverview(Int32 skippedCount, Int32 takenCount)
        {
            return _database.Guild.Read()
                .ToList()
                .Select(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor).ToGuildProfilePreviewDto())
                .OrderByDescending(g => g.Rating)
                .Skip(skippedCount)
                .Take(takenCount)
                .ToArray();
        }

        public GuildProfileDto Get(int id, int? userId)
        {
            return _database.Guild.Get(id)
                .To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .ToGuildProfileDto(userId);
        }

        public GuildProfileDto GetStudentGuild(int userId)
        {
            return _database.Guild.ReadForStudent(userId).To(g =>
                    new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .ToGuildProfileDto(userId);
        }

        public GithubRepository AddPinnedRepository(AuthorizedUser user, int guildId, string owner, string projectName)
        {
            GuildEntity guild = _database.Guild.Get(guildId);
            user.GetProfile(_database.Student).EnsureIsGuildEditor(guild);

            GithubRepository repository = _githubApiAccessor.GetRepository(owner, projectName);
            _database.Guild.PinProject(guildId, owner, projectName);
            return repository;
        }

        public void UnpinProject(AuthorizedUser user, int pinnedProjectId)
        {
            GuildPinnedProjectEntity guildPinnedProject = _database.Context.GuildPinnedProjects.Find(pinnedProjectId) ?? throw EntityNotFoundException.PinnedRepoWasNotFound(pinnedProjectId);
            GuildEntity guild = _database.Guild.ReadById(guildPinnedProject.GuildId);
            user.GetProfile(_database.Student).EnsureIsGuildEditor(guild);

            user.GetProfile(_database.Student).EnsureIsGuildEditor(guild);

            _database.Guild.UnpinProject(pinnedProjectId);
        }

        public GuildMemberLeaderBoard GetGuildMemberLeaderBoard(int guildId)
        {
            return _database.Guild.Get(guildId)
                .To(g => new GuildDomain(g, _database, _githubUserDataService, _githubApiAccessor))
                .GetMemberDashboard();
        }
    }
}