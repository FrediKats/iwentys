using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain;
using Iwentys.Domain.Enums;
using Iwentys.Domain.Models;
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

            List<ProjectReviewRequestInfoDto> reviewRequests = await testCase.ProjectReviewService.GetRequests(user);
            Assert.IsTrue(reviewRequests.Any(rr => rr.Project.Id == reviewRequest.Project.Id));
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

            reviewRequest = testCase.ProjectReviewService.GetRequests(user).Result.First(r => r.Id == reviewRequest.Id);

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

            reviewRequest = testCase.ProjectReviewService.GetRequests(user).Result.First(r => r.Id == reviewRequest.Id);
            Assert.AreEqual(ProjectReviewState.Finished, reviewRequest.State);
        }

        [Test]
        public async Task GetAvailableProjectForReview_NoActiveRequestProjects()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser reviewer = testCase.AccountManagementTestCaseContext.WithUser();
            GithubUser githubUser = testCase.GithubTestCaseContext.WithGithubAccount(user);
            GithubProject studentProject = testCase.GithubTestCaseContext.WithStudentProject(user);

            ProjectReviewRequestInfoDto reviewRequest = testCase.PeerReviewTestCaseContext.WithReviewRequest(user, studentProject);

            var projects = await testCase.ProjectReviewService.GetAvailableForReviewProject(user);

            Assert.IsEmpty(projects);
        }
    }
}