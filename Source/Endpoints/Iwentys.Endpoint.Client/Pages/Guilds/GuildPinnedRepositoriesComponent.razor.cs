using System.Threading.Tasks;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildPinnedRepositoriesComponent
    {
        public async Task RemovePin(long repositoryId)
        {
            await GuildClient.DeletePinnedProjectAsync(GuildProfile.Id, repositoryId);
            GuildProfile = await GuildClient.GetAsync(GuildProfile.Id);
        }
    }
}
