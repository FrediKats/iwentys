using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildPinnedRepositoriesCreatePage
    {
        private string _owner;
        private string _repositoryName;

        private async Task AddPin()
        {
            await GuildClient.AddPinnedProjectAsync(GuildId, new CreateProjectRequestDto {Owner = _owner, RepositoryName = _repositoryName});
            NavigationManager.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
