using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.PeerReview
{
    public partial class PeerReviewFeedbackCreatePage
    {
        private readonly List<ReviewFeedbackSummary> _feedbackSummaries = Enum.GetValues<ReviewFeedbackSummary>().ToList();

        public ProjectReviewRequestInfoDto _request;

        private string _description;
        private ReviewFeedbackSummary _selectedSummaries = ReviewFeedbackSummary.LooksGoodToMe;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ICollection<ProjectReviewRequestInfoDto> requests = await ClientHolder.ApiPeerReviewRequestsAllAsync();
            _request = requests.First(r => r.Id == ReviewRequestId);
        }

        private async Task CreateFeedback()
        {
            var arguments = new ReviewFeedbackCreateArguments
            {
                Description = _description,
                Summary = _selectedSummaries
            };

            await ClientHolder.ApiPeerReviewRequestsFeedbackAsync(ReviewRequestId, arguments);
            NavigationManager.NavigateTo("/peer-review");
        }
    }
}
