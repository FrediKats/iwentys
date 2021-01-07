using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GuildTestCaseContext
    {
        private readonly TestCaseContext _context;

        public GuildTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public ExtendedGuildProfileWithMemberDataDto WithGuild(AuthorizedUser user)
        {
            var guildCreateRequest = new GuildCreateRequestDto(null, null, null, GuildHiringPolicy.Close);

            GuildProfileShortInfoDto guild = _context.GuildService.Create(user, guildCreateRequest).Result;
            ExtendedGuildProfileWithMemberDataDto guildProfile = _context.GuildService.Get(guild.Id, user.Id).Result;
            return guildProfile;
        }

        public AuthorizedUser WithGuildMember(GuildProfileDto guild, AuthorizedUser guildEditor)
        {
            AuthorizedUser user = _context.AccountManagementTestCaseContext.WithUser();
            _context.GuildMemberService.RequestGuild(user, guild.Id).Wait();
            _context.GuildMemberService.AcceptRequest(guildEditor, guild.Id, user.Id).Wait();
            return user;
        }

        public AuthorizedUser WithGuildMentor(GuildProfileDto guild)
        {
            //TODO: make method for promoting to guild editor/mentor
            //TODO: remove direct call to DbContext
            AuthorizedUser user = _context.AccountManagementTestCaseContext.WithUser();
            _context._context.GuildMembers.Add(new GuildMember(guild.Id, user.Id, GuildMemberType.Mentor));
            _context._context.SaveChanges();
            return user;
        }

        public AuthorizedUser WithGuildRequest(GuildProfileDto guild)
        {
            AuthorizedUser user = _context.AccountManagementTestCaseContext.WithUser();
            _context.GuildMemberService.RequestGuild(user, guild.Id).Wait();
            return user;
        }

        public AuthorizedUser WithGuildBlocked(GuildProfileDto guild, AuthorizedUser guildEditor)
        {
            AuthorizedUser user = _context.AccountManagementTestCaseContext.WithUser();
            _context.GuildMemberService.RequestGuild(user, guild.Id).Wait();
            _context.GuildMemberService.BlockGuildMember(guildEditor, guild.Id, user.Id).Wait();
            return user;
        }
    }
}