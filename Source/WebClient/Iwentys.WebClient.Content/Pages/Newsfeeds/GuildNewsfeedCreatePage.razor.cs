using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Newsfeeds
{
    public partial class GuildNewsfeedCreatePage
    {
        private string _title;
        private string _description;

        private async Task ExecuteCreateNewsfeed()
        {
            await NewsfeedClient.CreateGuildNewsfeedAsync(GuildId, new NewsfeedCreateViewModel
            {
                Title = _title,
                Content = _description
            });

            NavigationManager.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
