using System.Threading.Tasks;
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
            await base.OnInitializedAsync();
            _groupProfile = await ClientHolder.StudyGroup.Get(GroupName);
        }

        private string LinkToStudentProfile(StudentInfoDto student) => $"student/profile/{student.Id}";
        private string LinkToSubjectProfile(Subject subject) => $"subject/{subject.Id}/profile";

        private StudentInfoDto GroupAdmin => _groupProfile?.GroupAdmin;
    }
}
