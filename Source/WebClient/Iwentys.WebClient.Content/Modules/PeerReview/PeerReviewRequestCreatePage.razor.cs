using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class PeerReviewRequestCreatePage
{
    private List<GithubRepositoryInfoDto> _availableForReviewProject;
    private GithubRepositoryInfoDto _selectedProject;
    private string _description;

    protected override async Task OnInitializedAsync()
    {
        _availableForReviewProject = new List<GithubRepositoryInfoDto>();
        _availableForReviewProject.AddRange(await _peerReviewClient.GetAvailableForReviewProjectAsync());
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

        await _peerReviewClient.CreateReviewRequestAsync(arguments);
        _navigationManager.NavigateTo("/peer-review");
    }
}