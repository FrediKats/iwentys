using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildRatePage : ComponentBase
    {
        private IReadOnlyList<GuildProfilePreviewDto> _guildProfiles;

        private string LinkToProfile(GuildProfilePreviewDto guild) => $"guild/profile/{guild.Id}";

        protected override async Task OnInitializedAsync()
        {
            var guildControllerClient = new GuildControllerClient(await Http.TrySetHeader(LocalStorage));
            _guildProfiles = await guildControllerClient.GetOverview();
        }
    }
}
