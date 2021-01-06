using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.PeerReview.Enums;
using Iwentys.Features.PeerReview.Models;

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
            ProjectReviewRequestInfoDto reviewRequest = _context.ProjectReviewService.CreateReviewRequest(user, new ReviewRequestCreateArguments {ProjectId = githubProject.Id}).Result;
            return reviewRequest;
        }

        public ProjectReviewFeedbackInfoDto WithReviewFeedback(AuthorizedUser user, ProjectReviewRequestInfoDto request)
        {
            ProjectReviewFeedbackInfoDto feedback = _context.ProjectReviewService.SendReviewFeedback(user, request.Id, new ReviewFeedbackCreateArguments {Summary = ReviewFeedbackSummary.LooksGoodToMe}).Result;
            return feedback;
        }
    }
}