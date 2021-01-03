using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Models;

namespace Iwentys.Endpoint.Client.Pages.Newsfeeds
{
    public partial class SubjectNewsfeedCreatePage
    {
        private string _title;
        private string _description;

        private async Task ExecuteCreateNewsfeed()
        {
            await ClientHolder.Newsfeed.CreateSubjectNewsfeed(SubjectId, new NewsfeedCreateViewModel
            {
                Title = _title,
                Content = _description
            });
            
            NavigationManager.NavigateTo($"/subject/{SubjectId}/profile");
        }
    }
}
