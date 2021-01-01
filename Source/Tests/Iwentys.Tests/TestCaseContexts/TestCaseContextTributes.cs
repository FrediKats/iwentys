using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Students.Domain;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithTribute(AuthorizedUser userInfo, CreateProjectRequestDto project, out TributeInfoResponse tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, project).Result;
            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, GithubProject project, out TributeInfoResponse tribute)
        {
            var userGithub = StudentService.GetAsync(userInfo.Id).Result.GithubUsername;

            tribute = GuildTributeServiceService.CreateTribute(
                    userInfo,
                    new CreateProjectRequestDto(userGithub, project.Name))
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