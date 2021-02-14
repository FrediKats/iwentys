using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildRatePage
    {
        private ICollection<GuildProfileDto> _guildProfiles;

        private string LinkToProfile(GuildProfileDto guild) => $"guild/profile/{guild.Id}";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _guildProfiles = await ClientHolder.ApiGuildGetAsync(null, null);
        }

        private async Task ForceUpdate()
        {
            await ClientHolder.ApiDebugcommandUpdateGuildImpactAsync();
            _guildProfiles = await ClientHolder.ApiGuildGetAsync(null, null);
        }
    }
}
