namespace Iwentys.WebClient.Content
{
    public partial class GuildPinnedRepositoriesComponent
    {
        public async Task RemovePin(long repositoryId)
        {
            //TODO: not implemented
            GuildProfile = await _guildClient.GetAsync(GuildProfile.Id);
        }
    }
}
