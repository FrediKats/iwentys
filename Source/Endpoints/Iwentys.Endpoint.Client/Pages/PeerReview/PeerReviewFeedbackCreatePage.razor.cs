using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.PeerReview.Enums;
using Iwentys.Features.PeerReview.Models;

namespace Iwentys.Endpoint.Client.Pages.PeerReview
{
    public partial class PeerReviewFeedbackCreatePage
    {
        public ProjectReviewRequestInfoDto _request;
        private string _description;

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
                //TODO: select
                Summary = ReviewFeedbackSummary.LooksGoodToMe
            };

            await ClientHolder.PeerReview.SendReviewFeedback(ReviewRequestId, arguments);
            NavigationManager.NavigateTo("/peer-review");
        }
    }
}
