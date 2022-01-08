using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class GuildNewsfeedCreatePage
{
    private string _title;
    private string _description;

    private async Task ExecuteCreateNewsfeed()
    {
        await _newsfeedClient.CreateGuildNewsfeedAsync(GuildId, new NewsfeedCreateViewModel
        {
            Title = _title,
            Content = _description
        });

        _navigationManager.NavigateTo($"/guild/profile/{GuildId}");
    }
}