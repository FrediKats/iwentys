using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Students
{
    public partial class StudentProfileEditPage
    {
        private StudentInfoDto _studentFullProfile;

        private string _githubUsername;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _studentFullProfile = await StudentClient.GetSelfAsync();
            _githubUsername = _studentFullProfile.GithubUsername;
        }

        private async Task OnSave()
        {
            if (_githubUsername is not null && _studentFullProfile.GithubUsername != _githubUsername)
            {
                await StudentClient.UpdateProfileAsync(new StudentUpdateRequestDto {GithubUsername = _githubUsername });
            }
        }
    }
}
