using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Models.Guilds;

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
