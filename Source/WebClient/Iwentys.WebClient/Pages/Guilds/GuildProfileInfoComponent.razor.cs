using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.Guilds
{
    public partial class GuildProfileInfoComponent
    {
        private UserMembershipState? _membership;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //TODO: null value handling
            _membership = await GuildMembershipClient.GetSelfMembershipAsync(SelectedGuildProfile.Id);
        }

        private async Task LeaveGuild()
        {
            await GuildMembershipClient.LeaveAsync(SelectedGuildProfile.Id);
            NavigationManager.NavigateTo("/assignment");
        }
    }
}
