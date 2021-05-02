using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Enums;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.PeerReview
{
    [TestFixture]
    public class ProjectReviewServiceTest
    {
        [Test]
        //TODO: no asserts
        public void GetAvailableProjectForReview_ShouldExistForNewProject()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser user = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            GithubUser githubUser = new GithubUser { IwentysUserId = user.Id, Username = user.GithubUsername };
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(user.GithubUsername);
            var githubProject = new GithubProject(githubUser, repositoryInfo);
        }

        [Test]
        public void CreateReviewRequest_RequestExists()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser user = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            GithubUser githubUser = new GithubUser { IwentysUserId = user.Id, Username = user.GithubUsername };
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(user.GithubUsername);
            var githubProject = new GithubProject(githubUser, repositoryInfo);

            var createArguments = new ReviewRequestCreateArguments { ProjectId = githubProject.Id, Visibility = ProjectReviewVisibility.Open };
            var projectReviewRequest = ProjectReviewRequest.Create(user, new GithubRepositoryInfoDto(githubProject), createArguments);

            Assert.IsTrue(projectReviewRequest.AuthorId == user.Id);
        }

        [Test]
        public void SendReviewFeedback_RequestHasFeedback()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser user = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            GithubUser githubUser = new GithubUser { IwentysUserId = user.Id, Username = user.GithubUsername };
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(user.GithubUsername);
            var githubProject = new GithubProject(githubUser, repositoryInfo);
            var createArguments = new ReviewRequestCreateArguments { ProjectId = githubProject.Id, Visibility = ProjectReviewVisibility.Open };
            var projectReviewRequest = ProjectReviewRequest.Create(user, new GithubRepositoryInfoDto(githubProject), createArguments);

            AuthorizedUser reviewer = testCase.AccountManagementTestCaseContext.WithUser();
            var reviewFeedbackCreateArguments = new ReviewFeedbackCreateArguments { Summary = ReviewFeedbackSummary.LooksGoodToMe };
            ProjectReviewFeedback projectReviewFeedback = projectReviewRequest.CreateFeedback(reviewer, reviewFeedbackCreateArguments);

            Assert.IsTrue(projectReviewFeedback.AuthorId == reviewer.Id);
        }

        [Test]
        public void FinishReviewFeedback_StateChanged()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser user = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            GithubUser githubUser = new GithubUser { IwentysUserId = user.Id, Username = user.GithubUsername };
            GithubRepositoryInfoDto repositoryInfo = GithubRepositoryFaker.Instance.Generate(user.GithubUsername);
            var githubProject = new GithubProject(githubUser, repositoryInfo);
            var createArguments = new ReviewRequestCreateArguments { ProjectId = githubProject.Id, Visibility = ProjectReviewVisibility.Open };
            var projectReviewRequest = ProjectReviewRequest.Create(user, new GithubRepositoryInfoDto(githubProject), createArguments);

            projectReviewRequest.FinishReview(user);

            Assert.AreEqual(ProjectReviewState.Finished, projectReviewRequest.State);
        }
    }
}