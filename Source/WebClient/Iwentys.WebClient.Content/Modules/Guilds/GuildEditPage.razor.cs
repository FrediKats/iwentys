namespace Iwentys.WebClient.Content;

public partial class GuildEditPage
{
    protected override async Task OnInitializedAsync()
    {
        _guild = await _guildClient.GetAsync(GuildId);
    }
}