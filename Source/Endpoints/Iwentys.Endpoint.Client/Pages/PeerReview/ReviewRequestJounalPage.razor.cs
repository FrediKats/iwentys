using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.PeerReview.Models;

namespace Iwentys.Endpoint.Client.Pages.PeerReview
{
    public partial class ReviewRequestJounalPage
    {
        private List<ProjectReviewRequestInfoDto> _projectReviewRequests;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _projectReviewRequests = await ClientHolder.PeerReview.Get();
        }
    }
}
