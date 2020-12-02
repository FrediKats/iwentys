using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Integrations.GithubIntegration.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class PinnedRepositoryCreatePage
    {
        private string _owner;
        private string _repositoryName;

        private async Task AddPin()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            var guildControllerClient = new GuildControllerClient(httpClient);
            GithubRepository project = await guildControllerClient.AddPinnedProject(GuildId, new CreateProjectRequest {Owner = _owner, RepositoryName = _repositoryName});
            Navigation.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
