using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Guilds;

namespace Iwentys.Tests.TestCaseContexts
{
    public class TributeTestCaseContext
    {
        private readonly TestCaseContext _context;

        public TributeTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public Tribute WithTribute(IwentysUser user, GithubProject project)
        {
            Guild guild = _context._context.GuildMembers.ReadForStudent(user.Id);
            List<Tribute> allTributes = _context._context.Tributes.ToList();

            var tribute = Tribute.Create(guild, user, project, allTributes);
            return tribute;
        }

        public Tribute CompleteTribute(AuthorizedUser mentor, Tribute tribute)
        {
            tribute.SetCompleted(mentor.Id, GuildFaker.Instance.NewTributeCompleteRequest(tribute.Project.Id));
            return tribute;
        }
    }
}