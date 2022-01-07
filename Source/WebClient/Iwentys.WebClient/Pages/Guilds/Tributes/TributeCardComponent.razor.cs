namespace Iwentys.WebClient.Pages.Guilds.Tributes
{
    public partial class TributeCardComponent
    {
        public void Complete()
        {
            NavigationManagerClient.NavigateTo($"/guild/{Tribute.GuildId}/tribute/{Tribute.Project.Id}/response");
        }
    }
}
