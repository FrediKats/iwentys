namespace Iwentys.WebClient.Content
{
    public partial class GuildPinnedRepositoriesCreatePage
    {
        private string _owner;
        private string _repositoryName;

        private void AddPin()
        {
            //TODO: not implemented
            _navigationManager.NavigateTo($"/guild/profile/{GuildId}");
        }
    }
}
