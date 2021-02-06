using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Newsfeeds
{
    public partial class SubjectNewsfeedCreatePage
    {
        private string _title;
        private string _description;

        private async Task ExecuteCreateNewsfeed()
        {
            await ClientHolder.ApiNewsfeedSubjectPostAsync(SubjectId, new NewsfeedCreateViewModel
            {
                Title = _title,
                Content = _description
            });
            
            NavigationManager.NavigateTo($"/subject/{SubjectId}/profile");
        }
    }
}
