using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.Guilds
{
    public partial class GuildRatePage
    {
        private ICollection<GuildProfileDto> _guildProfiles;

        private string LinkToProfile(GuildProfileDto guild) => $"guild/profile/{guild.Id}";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _guildProfiles = await GuildClient.GetRankedAsync(null, null);
        }

        private async Task ForceUpdate()
        {
            await DebugCommandClient.UpdateGuildImpactAsync();
            _guildProfiles = await GuildClient.GetRankedAsync(null, null);
        }
    }
}
