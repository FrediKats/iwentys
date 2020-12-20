using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePinnedRepositoriesComponent
    {
        private GuildControllerClient _guildControllerClient;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _guildControllerClient = new GuildControllerClient(httpClient);
        }

        public async Task RemovePin(long repositoryId)
        {
            await _guildControllerClient.DeletePinnedProject(GuildProfile.Id, repositoryId);
            GuildProfile = await _guildControllerClient.Get(GuildProfile.Id);
        }
    }
}
