using System.Linq;
using Iwentys.Database.Seeding.FakerEntities.Guilds;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Domain.PeerReview.Dto;
using Microsoft.EntityFrameworkCore;

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
            IwentysUserInfoDto result = _context.UnitOfWork.GetRepository<IwentysUser>()
                .Get()
                .Where(u => u.Id == userInfo.Id)
                .Select(u => new IwentysUserInfoDto(u))
                .SingleAsync().Result;
            return _context.GuildTributeServiceService.CreateTribute(userInfo, new CreateProjectRequestDto(result.GithubUsername, project.Name)).Result;
        }

        public TributeInfoResponse CompleteTribute(AuthorizedUser mentor, TributeInfoResponse tribute)
        {
            return _context.GuildTributeServiceService.CompleteTribute(mentor, GuildFaker.Instance.NewTributeCompleteRequest(tribute.Project.Id)).Result;
        }
    }
}