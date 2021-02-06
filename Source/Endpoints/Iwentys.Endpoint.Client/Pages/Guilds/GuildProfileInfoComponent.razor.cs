using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfileInfoComponent
    {
        private UserMembershipState? _membership;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //TODO: null value handling
            _membership = await ClientHolder.ApiGuildMembershipAsync(SelectedGuildProfile.Id);
        }

        private async Task LeaveGuild()
        {
            await ClientHolder.GuildMember.Leave(SelectedGuildProfile.Id);
            NavigationManager.NavigateTo("/assignment");
        }
    }
}
