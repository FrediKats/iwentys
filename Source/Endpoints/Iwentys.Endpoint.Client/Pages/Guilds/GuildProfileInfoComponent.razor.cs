using System.Threading.Tasks;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfileInfoComponent
    {
        private async Task LeaveGuild()
        {
            await ClientHolder.GuildMember.Leave(SelectedGuildProfile.Id);
            NavigationManager.NavigateTo("/assignment");
        }
    }
}
