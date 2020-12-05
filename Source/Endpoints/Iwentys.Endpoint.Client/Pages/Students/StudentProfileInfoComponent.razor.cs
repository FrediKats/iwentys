using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.StudentFeature.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfileInfoComponent : ComponentBase
    {
        private GuildProfileDto _guild;
        private GroupProfileResponse _group;

        protected override async Task OnInitializedAsync()
        {
            var httpClient = await Http.TrySetHeader(LocalStorage);
            var studentControllerClient = new GuildControllerClient(httpClient);
            var studyGroupControllerClient = new StudyGroupControllerClient(httpClient);

            _guild = await studentControllerClient.GetForMember(StudentProfile.Id);
            _group = await studyGroupControllerClient.GetStudentGroup(StudentProfile.Id);
        }

        private string LinkToGuild() => $"guild/profile/{_guild.Id}";
    }
}
