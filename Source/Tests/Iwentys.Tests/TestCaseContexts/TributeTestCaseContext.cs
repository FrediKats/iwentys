using Iwentys.Database.Seeding.FakerEntities.Guilds;
using Iwentys.Domain;
using Iwentys.Domain.Models;

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
            IwentysUserInfoDto result = _context.IwentysUserService.Get(userInfo.Id).Result;
            return _context.GuildTributeServiceService.CreateTribute(userInfo, new CreateProjectRequestDto(result.GithubUsername, project.Name)).Result;
        }

        public TributeInfoResponse CompleteTribute(AuthorizedUser mentor, TributeInfoResponse tribute)
        {
            return _context.GuildTributeServiceService.CompleteTribute(mentor, GuildFaker.Instance.NewTributeCompleteRequest(tribute.Project.Id)).Result;
        }
    }
}