using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GuildTestCaseContext
    {
        private readonly TestCaseContext _context;

        public GuildTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public GuildProfileDto WithGuild(IwentysUser user)
        {
            return WithGuild(AuthorizedUser.DebugAuth(user.Id));
        }

        public GuildProfileDto WithGuild(AuthorizedUser user)
        {
            var guildCreateRequest = new GuildCreateRequestDto(null, null, null, GuildHiringPolicy.Close);

            GuildProfileShortInfoDto guild = _context.GuildService.Create(user, guildCreateRequest).Result;
            GuildProfileDto guildProfile = _context.GuildService.Get(guild.Id).Result;
            return guildProfile;
        }

        public AuthorizedUser WithGuildRequest(GuildProfileDto guild)
        {
            AuthorizedUser user = _context.AccountManagementTestCaseContext.WithUser();
            _context.GuildMemberService.RequestGuild(user, guild.Id).Wait();
            return user;
        }
    }
}