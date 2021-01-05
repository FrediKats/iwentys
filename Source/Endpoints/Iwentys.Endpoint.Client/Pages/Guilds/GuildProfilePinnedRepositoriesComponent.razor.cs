using System.Threading.Tasks;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfilePinnedRepositoriesComponent
    {
        public async Task RemovePin(long repositoryId)
        {
            await ClientHolder.Guild.DeletePinnedProject(GuildProfile.Id, repositoryId);
            GuildProfile = await ClientHolder.Guild.Get(GuildProfile.Id);
        }
    }
}
