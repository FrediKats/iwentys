using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class GuildRatePage
{
    private ICollection<GuildProfileDto> _guildProfiles;

    private string LinkToProfile(GuildProfileDto guild) => $"guild/profile/{guild.Id}";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _guildProfiles = await _guildClient.GetRankedAsync(null, null);
    }

    private async Task ForceUpdate()
    {
        await _debugCommandClient.UpdateGuildImpactAsync();
        _guildProfiles = await _guildClient.GetRankedAsync(null, null);
    }
}