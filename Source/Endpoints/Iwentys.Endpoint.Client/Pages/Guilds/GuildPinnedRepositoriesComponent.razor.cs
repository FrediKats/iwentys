using System.Threading.Tasks;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildPinnedRepositoriesComponent
    {
        public async Task RemovePin(long repositoryId)
        {
            await ClientHolder.ApiGuildPinnedDeleteAsync(GuildProfile.Id, repositoryId);
            GuildProfile = await ClientHolder.ApiGuildGetAsync(GuildProfile.Id);
        }
    }
}
