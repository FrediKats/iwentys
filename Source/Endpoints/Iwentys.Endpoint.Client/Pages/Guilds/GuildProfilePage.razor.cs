using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePage : ComponentBase
    {
        private GuildProfileDto _guild;

        protected override async Task OnInitializedAsync()
        {
            var guildControllerClient = new GuildControllerClient(await Http.TrySetHeader(LocalStorage));
            _guild = await guildControllerClient.Get(GuildId);
        }
    }
}
