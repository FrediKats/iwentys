using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.PeerReview.Enums;
using Iwentys.Features.PeerReview.Models;

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

            List<ProjectReviewRequestInfoDto> requests = await ClientHolder.PeerReview.Get();
            _request = requests.Find(r => r.Id == ReviewRequestId);
        }

        private async Task CreateFeedback()
        {
            var arguments = new ReviewFeedbackCreateArguments
            {
                Description = _description,
                Summary = _selectedSummaries
            };

            await ClientHolder.PeerReview.SendReviewFeedback(ReviewRequestId, arguments);
            NavigationManager.NavigateTo("/peer-review");
        }
    }
}
