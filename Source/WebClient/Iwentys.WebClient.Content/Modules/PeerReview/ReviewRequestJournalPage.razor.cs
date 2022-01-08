using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class ReviewRequestJournalPage
    {
        private ICollection<ProjectReviewRequestInfoDto> _projectReviewRequests;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _projectReviewRequests = await _peerReviewClient.GetProjectReviewRequestsAsync();
        }

        public static string LinkToReviewRequestCreatePage() => "peer-review/create";
        public static string LinkToSendingFeedback(ProjectReviewRequestInfoDto request) => $"/peer-review/{request.Id}/feedback";
    }
}
