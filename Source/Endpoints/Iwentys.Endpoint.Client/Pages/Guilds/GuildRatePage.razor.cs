using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildRatePage
    {
        private IReadOnlyList<GuildProfileDto> _guildProfiles;

        private string LinkToProfile(GuildProfileDto guild) => $"guild/profile/{guild.Id}";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _guildProfiles = await ClientHolder.Guild.GetOverview();
        }
    }
}
