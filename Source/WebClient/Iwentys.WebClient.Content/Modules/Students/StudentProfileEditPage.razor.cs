using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class StudentProfileEditPage
    {
        private StudentInfoDto _studentFullProfile;

        private string _githubUsername;

        protected override async Task OnInitializedAsync()
        {
            _studentFullProfile = await _studentClient.GetSelfAsync();
            _githubUsername = _studentFullProfile.GithubUsername;
        }

        private async Task OnSave()
        {
            if (_githubUsername is not null && _studentFullProfile.GithubUsername != _githubUsername)
            {
                await _studentClient.UpdateProfileAsync(new StudentUpdateRequestDto {GithubUsername = _githubUsername });
            }
        }
    }
}
