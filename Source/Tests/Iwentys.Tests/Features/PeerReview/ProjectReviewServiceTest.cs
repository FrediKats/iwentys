using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.PeerReview.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.PeerReview
{
    [TestFixture]
    public class ProjectReviewServiceTest
    {
        [Test]
        public async Task CreateReviewRequest_RequestExists()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            GithubUser githubUser = testCase.GithubTestCaseContext.WithGithubAccount(user);
            GithubProject studentProject = testCase.GithubTestCaseContext.WithStudentProject(user);

            ProjectReviewRequestInfoDto reviewRequest = testCase.PeerReviewTestCaseContext.WithReviewRequest(user, studentProject);

            List<ProjectReviewRequestInfoDto> reviewRequests = await testCase.ProjectReviewService.GetRequests();
            Assert.IsTrue(reviewRequests.Any(rr => rr.ProjectId == reviewRequest.ProjectId));
        }
    }
}