using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Features.Guilds.Models.Guilds;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfileInfoComponent : ComponentBase
    {
        private GuildProfileDto _guild;

        protected override async Task OnInitializedAsync()
        {
            var studentControllerClient = new GuildControllerClient(await Http.TrySetHeader(LocalStorage));

            _guild = await studentControllerClient.GetForMember(StudentProfile.Id);
        }

        private string LinkToGuild() => $"guild/profile/{_guild.Id}";
    }
}
