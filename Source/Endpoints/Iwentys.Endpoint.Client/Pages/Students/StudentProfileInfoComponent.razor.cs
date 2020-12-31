using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfileInfoComponent : ComponentBase
    {
        private GuildProfileDto _guild;
        private GroupProfileResponseDto _group;
        private StudentInfoDto _self;

        private StudyGroupControllerClient _studyGroupControllerClient;

        protected override async Task OnInitializedAsync()
        {
            var httpClient = await Http.TrySetHeader(LocalStorage);
            var guildControllerClient = new GuildControllerClient(httpClient);
            var studentControllerClient = new StudentControllerClient(Http);
            _studyGroupControllerClient = new StudyGroupControllerClient(httpClient);

            _guild = await guildControllerClient.GetForMember(StudentProfile.Id);
            _group = await _studyGroupControllerClient.FindStudentGroup(StudentProfile.Id);
            _self = await studentControllerClient.GetSelf();
        }

        private string LinkToGuild => $"guild/profile/{_guild.Id}";
        private string LinkToGroupProfile => $"/group/profile/{_group.GroupName}";

        private Task MakeGroupAdmin()
        {
            return _studyGroupControllerClient.MakeGroupAdmin(StudentProfile.Id);
        }
    }
}
