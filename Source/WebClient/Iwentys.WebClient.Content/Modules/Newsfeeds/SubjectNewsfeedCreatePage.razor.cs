using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class SubjectNewsfeedCreatePage
    {
        private string _title;
        private string _description;

        private async Task ExecuteCreateNewsfeed()
        {
            await _newsfeedClient.CreateSubjectNewsfeedAsync(SubjectId, new NewsfeedCreateViewModel
            {
                Title = _title,
                Content = _description
            });
            
            _navigationManager.NavigateTo($"/subject/{SubjectId}/profile");
        }
    }
}
