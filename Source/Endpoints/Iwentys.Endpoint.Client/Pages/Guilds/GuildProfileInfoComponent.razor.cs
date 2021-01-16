using System.Threading.Tasks;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Endpoint.Client.Pages.Guilds
{
    public partial class GuildProfileInfoComponent
    {
        private UserMembershipState? _membership;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _membership = await ClientHolder.GuildMember.GetUserMembership(SelectedGuildProfile.Id);
        }

        private async Task LeaveGuild()
        {
            await ClientHolder.GuildMember.Leave(SelectedGuildProfile.Id);
            NavigationManager.NavigateTo("/assignment");
        }
    }
}
