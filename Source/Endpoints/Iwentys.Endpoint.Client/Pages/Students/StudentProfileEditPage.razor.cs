using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Students.Models;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfileEditPage
    {
        private StudentInfoDto _studentFullProfile;

        private string _githubUsername;

        private StudentControllerClient _studentControllerClient;

        protected override async Task OnInitializedAsync()
        {
            var httpClient = await Http.TrySetHeader(LocalStorage);
            _studentControllerClient = new StudentControllerClient(httpClient);

            _studentFullProfile = await _studentControllerClient.GetSelf();
            _githubUsername = _studentFullProfile.GithubUsername;
        }

        private async Task OnSave()
        {
            if (_githubUsername is not null && _studentFullProfile.GithubUsername != _githubUsername)
            {
                await _studentControllerClient.Update(new StudentUpdateRequestDto(_githubUsername));
            }
            else
            {
                //TODO: alert?
            }
        }
    }
}
