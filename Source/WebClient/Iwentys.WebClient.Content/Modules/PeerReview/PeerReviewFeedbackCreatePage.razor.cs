using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class PeerReviewFeedbackCreatePage
    {
        private readonly List<ReviewFeedbackSummary> _feedbackSummaries = Enum.GetValues<ReviewFeedbackSummary>().ToList();

        public ProjectReviewRequestInfoDto _request;

        private string _description;
        private ReviewFeedbackSummary _selectedSummaries = ReviewFeedbackSummary.LooksGoodToMe;

        protected override async Task OnInitializedAsync()
        {
            ICollection<ProjectReviewRequestInfoDto> requests = await _peerReviewClient.GetProjectReviewRequestsAsync();
            _request = requests.First(r => r.Id == ReviewRequestId);
        }

        private async Task CreateFeedback()
        {
            var arguments = new ReviewFeedbackCreateArguments
            {
                Description = _description,
                Summary = _selectedSummaries
            };

            await _peerReviewClient.SendReviewFeedbackAsync(ReviewRequestId, arguments);
            _navigationManager.NavigateTo("/peer-review");
        }
    }
}
