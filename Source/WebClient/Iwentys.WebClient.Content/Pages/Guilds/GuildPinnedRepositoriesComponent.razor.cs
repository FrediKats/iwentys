namespace Iwentys.WebClient.Content.Pages.Guilds
{
    public partial class GuildPinnedRepositoriesComponent
    {
        public async Task RemovePin(long repositoryId)
        {
            //TODO: not implemented
            GuildProfile = await GuildClient.GetAsync(GuildProfile.Id);
        }
    }
}
