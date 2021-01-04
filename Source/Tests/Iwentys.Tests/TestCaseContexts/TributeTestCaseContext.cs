using Iwentys.Database.Seeding.FakerEntities.Guilds;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tributes.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class TributeTestCaseContext
    {
        private readonly TestCaseContext _context;

        public TributeTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public TributeInfoResponse WithTribute(AuthorizedUser userInfo, GithubProject project)
        {
            var userGithub = _context.StudentService.Get(userInfo.Id).Result.GithubUsername;
            return _context.GuildTributeServiceService.CreateTribute(userInfo, new CreateProjectRequestDto(userGithub, project.Name)).Result;
        }

        public TributeInfoResponse CompleteTribute(AuthorizedUser mentor, TributeInfoResponse tribute)
        {
            return _context.GuildTributeServiceService.CompleteTribute(mentor, GuildTributeFaker.Instance.NewTributeCompleteRequest(tribute.Project.Id)).Result;
        }
    }
}