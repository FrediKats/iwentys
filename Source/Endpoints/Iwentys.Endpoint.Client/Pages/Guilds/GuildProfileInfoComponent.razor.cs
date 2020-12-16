using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfileInfoComponent
    {
        private GuildControllerClient _guildControllerClient;
        private GuildMemberControllerClient _guildMemberControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
            _guildMemberControllerClient = new GuildMemberControllerClient(httpClient);
        }

        private async Task LeaveGuild()
        {
            await _guildMemberControllerClient.Leave(SelectedGuildProfile.Id);
            NavigationManagerClient.NavigateTo("/assignment");
        }
    }
}
