using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Pages.Study
{
    public partial class GroupPage
    {
        private GroupProfileResponseDto _groupProfile;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _groupProfile = await StudyGroupClient.GetByGroupNameAsync(GroupName);
        }

        private string LinkToStudentProfile(StudentInfoDto student) => $"student/profile/{student.Id}";
        private string LinkToSubjectProfile(SubjectProfileDto subject) => $"subject/{subject.Id}/profile";

        private StudentInfoDto GroupAdmin => _groupProfile?.GroupAdmin;
    }
}
