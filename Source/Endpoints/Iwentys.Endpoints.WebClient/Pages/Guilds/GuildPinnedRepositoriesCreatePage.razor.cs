using System.Threading.Tasks;

namespace Iwentys.Endpoints.WebClient.Pages.Guilds
{
    public partial class GuildPinnedRepositoriesCreatePage
    {
        private string _owner;
        private string _repositoryName;

        private async Task AddPin()
        {
            //TODO: not implemented
            NavigationManager.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
