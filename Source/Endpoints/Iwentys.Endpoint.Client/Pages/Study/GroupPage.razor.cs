using System.Threading.Tasks;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Models.Students;

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
        private string LinkToSubjectProfile(SubjectProfileDto subject) => $"subject/{subject.Id}/profile";

        private StudentInfoDto GroupAdmin => _groupProfile?.GroupAdmin;
    }
}
