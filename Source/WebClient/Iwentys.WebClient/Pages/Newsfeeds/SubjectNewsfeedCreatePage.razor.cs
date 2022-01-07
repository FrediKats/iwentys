using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.Newsfeeds
{
    public partial class SubjectNewsfeedCreatePage
    {
        private string _title;
        private string _description;

        private async Task ExecuteCreateNewsfeed()
        {
            await NewsfeedClient.CreateSubjectNewsfeedAsync(SubjectId, new NewsfeedCreateViewModel
            {
                Title = _title,
                Content = _description
            });
            
            NavigationManager.NavigateTo($"/subject/{SubjectId}/profile");
        }
    }
}
