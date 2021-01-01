using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class GroupPage
    {
        private GroupProfileResponseDto _groupProfile;

        protected override async Task OnInitializedAsync()
        {
            var studentControllerClient = new StudyGroupControllerClient(await Http.TrySetHeader(LocalStorage));

            _groupProfile = await studentControllerClient.Get(GroupName);
        }

        private string LinkToStudentProfile(StudentInfoDto student) => $"student/profile/{student.Id}";
        private string LinkToSubjectProfile(Subject subject) => $"subject/profile/{subject.Id}";

        private StudentInfoDto GroupAdmin => _groupProfile?.GroupAdmin;
    }
}
