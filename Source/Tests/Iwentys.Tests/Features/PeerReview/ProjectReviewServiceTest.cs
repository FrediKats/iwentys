using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.PeerReview.Enums;
using Iwentys.Features.PeerReview.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.PeerReview
{
    [TestFixture]
    public class ProjectReviewServiceTest
    {
        [Test]
        public async Task GetAvailableProjectForReview_ShouldExistForNewProject()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            GithubUser githubUser = testCase.GithubTestCaseContext.WithGithubAccount(user);
            GithubProject studentProject = testCase.GithubTestCaseContext.WithStudentProject(user);

            List<GithubRepositoryInfoDto> projects = await testCase.ProjectReviewService.GetAvailableForReviewProject(user);

            Assert.IsTrue(projects.Any(p => p.Id == studentProject.Id));
        }

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

        [Test]
        public async Task SendReviewFeedback_RequestHasFeedback()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser reviewer = testCase.AccountManagementTestCaseContext.WithUser();
            GithubUser githubUser = testCase.GithubTestCaseContext.WithGithubAccount(user);
            GithubProject studentProject = testCase.GithubTestCaseContext.WithStudentProject(user);

            ProjectReviewRequestInfoDto reviewRequest = testCase.PeerReviewTestCaseContext.WithReviewRequest(user, studentProject);
            ProjectReviewFeedbackInfoDto feedback = testCase.PeerReviewTestCaseContext.WithReviewFeedback(reviewer, reviewRequest);

            reviewRequest = testCase.ProjectReviewService.GetRequests().Result.First(r => r.Id == reviewRequest.Id);

            Assert.IsTrue(reviewRequest.ReviewFeedbacks.Any(rf => rf.Id == feedback.Id));
        }

        [Test]
        public async Task FinishReviewFeedback_StateChanged()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser reviewer = testCase.AccountManagementTestCaseContext.WithUser();
            GithubUser githubUser = testCase.GithubTestCaseContext.WithGithubAccount(user);
            GithubProject studentProject = testCase.GithubTestCaseContext.WithStudentProject(user);

            ProjectReviewRequestInfoDto reviewRequest = testCase.PeerReviewTestCaseContext.WithReviewRequest(user, studentProject);

            await testCase.ProjectReviewService.FinishReview(user, reviewRequest.Id);

            reviewRequest = testCase.ProjectReviewService.GetRequests().Result.First(r => r.Id == reviewRequest.Id);
            Assert.AreEqual(ProjectReviewState.Finished, reviewRequest.State);
        }
    }
}