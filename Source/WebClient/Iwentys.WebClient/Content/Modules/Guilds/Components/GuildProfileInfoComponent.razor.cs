using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class GuildProfileInfoComponent
{
    private UserMembershipState? _membership;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        //TODO: null value handling
        _membership = await _guildMembershipClient.GetSelfMembershipAsync(SelectedGuildProfile.Id);
    }

    private async Task LeaveGuild()
    {
        await _guildMembershipClient.LeaveAsync(SelectedGuildProfile.Id);
        _navigationManager.NavigateTo("/assignment");
    }
}