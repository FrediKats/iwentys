using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended.Enums;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.GithubIntegration;

namespace Iwentys.Tests.TestCaseContexts
{
    public class PeerReviewTestCaseContext
    {
        private readonly TestCaseContext _context;

        public PeerReviewTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public ProjectReviewRequestInfoDto WithReviewRequest(AuthorizedUser user, GithubProject githubProject)
        {
            var createArguments = new ReviewRequestCreateArguments { ProjectId = githubProject.Id, Visibility = ProjectReviewVisibility.Open};
            ProjectReviewRequestInfoDto reviewRequest = _context.ProjectReviewService.CreateReviewRequest(user, createArguments).Result;
            return reviewRequest;
        }

        public ProjectReviewFeedbackInfoDto WithReviewFeedback(AuthorizedUser user, ProjectReviewRequestInfoDto request)
        {
            ProjectReviewFeedbackInfoDto feedback = _context.ProjectReviewService.SendReviewFeedback(user, request.Id, new ReviewFeedbackCreateArguments {Summary = ReviewFeedbackSummary.LooksGoodToMe}).Result;
            return feedback;
        }
    }
}