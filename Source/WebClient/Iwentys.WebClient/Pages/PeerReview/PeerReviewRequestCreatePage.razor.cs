using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.PeerReview
{
    public partial class PeerReviewRequestCreatePage
    {
        private List<GithubRepositoryInfoDto> _availableForReviewProject;
        private GithubRepositoryInfoDto _selectedProject;
        private string _description;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _availableForReviewProject = new List<GithubRepositoryInfoDto>();
            _availableForReviewProject.AddRange(await PeerReviewClient.GetAvailableForReviewProjectAsync());
            //FYI: this value is used in selector
            _availableForReviewProject.Insert(0, null);
        }

        private async Task CreateRequest()
        {
            var arguments = new ReviewRequestCreateArguments
            {
                ProjectId = _selectedProject.Id,
                Description = _description,
                Visibility = ProjectReviewVisibility.Open
            };

            await PeerReviewClient.CreateReviewRequestAsync(arguments);
            NavigationManager.NavigateTo("/peer-review");
        }
    }
}
