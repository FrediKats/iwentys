using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class GuildTributeJournalPage
{
    private GuildProfileDto _guild;
    private ICollection<TributeInfoResponse> _tributes;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
            
        _guild = await _guildClient.GetAsync(GuildId);
        _tributes = await _guildTributeClient.GetByGuildIdAsync(GuildId);
    }
}