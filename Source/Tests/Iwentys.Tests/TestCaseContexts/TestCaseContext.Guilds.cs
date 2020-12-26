using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Tributes.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithGuild(AuthorizedUser user, out ExtendedGuildProfileWithMemberDataDto guildProfile)
        {
            var guildCreateRequest = new GuildCreateRequestDto(null, null, null, GuildHiringPolicy.Close);

            GuildProfileShortInfoDto guild = GuildService.CreateAsync(user, guildCreateRequest).Result;
            guildProfile = GuildService.GetAsync(guild.Id, user.Id).Result;
            return this;
        }

        public TestCaseContext WithGuildMember(GuildProfileDto guild, AuthorizedUser guildEditor, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            GuildMemberService.RequestGuildAsync(user, guild.Id).Wait();
            GuildMemberService.AcceptRequest(guildEditor, guild.Id, user.Id).Wait();
            return this;
        }

        public TestCaseContext WithGuildMentor(GuildProfileDto guild, out AuthorizedUser user)
        {
            //TODO: make method for promoting to guild editor/mentor
            WithNewStudent(out user);
            _context.GuildMembers.Add(new GuildMemberEntity(guild.Id, user.Id, GuildMemberType.Mentor));
            _context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildRequest(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            GuildMemberService.RequestGuildAsync(user, guild.Id).Wait();
            return this;
        }

        public TestCaseContext WithGuildBlocked(GuildProfileDto guild, AuthorizedUser guildEditor, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            GuildMemberService.RequestGuildAsync(user, guild.Id).Wait();
            GuildMemberService.BlockGuildMember(guildEditor, guild.Id, user.Id).Wait();
            return this;
        }
        public TestCaseContext WithTribute(AuthorizedUser userInfo, CreateProjectRequestDto project, out TributeInfoResponse tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, project).Result;
            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, GithubProjectEntity projectEntity, out TributeInfoResponse tribute)
        {
            var userGithub = StudentService.GetAsync(userInfo.Id).Result.GithubUsername;

            tribute = GuildTributeServiceService.CreateTribute(
                    userInfo,
                    new CreateProjectRequestDto(userGithub, projectEntity.Name))
                .Result;
            return this;
        }

        public TestCaseContext WithCompletedTribute(AuthorizedUser mentor, TributeInfoResponse tribute, out TributeInfoResponse completedTribute)
        {
            completedTribute = GuildTributeServiceService.CompleteTribute(mentor, new TributeCompleteRequest
            {
                DifficultLevel = 1,
                Mark = 1,
                TributeId = tribute.Project.Id
            }).Result;
            return this;
        }
    }
}