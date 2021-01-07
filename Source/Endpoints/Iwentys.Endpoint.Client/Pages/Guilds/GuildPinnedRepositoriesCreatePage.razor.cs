using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildPinnedRepositoriesCreatePage
    {
        private string _owner;
        private string _repositoryName;

        private async Task AddPin()
        {
            await ClientHolder.Guild.AddPinnedProject(GuildId, new CreateProjectRequestDto(_owner, _repositoryName));
            NavigationManager.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
