namespace Iwentys.WebClient.Content
{
    public partial class TributeCardComponent
    {
        public void Complete()
        {
            _navigationManager.NavigateTo($"/guild/{Tribute.GuildId}/tribute/{Tribute.Project.Id}/response");
        }
    }
}
