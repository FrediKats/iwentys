using System.Threading.Tasks;
using Iwentys.Features.Students.Models;

namespace Iwentys.Endpoint.Client.Pages.Students
{
    public partial class StudentProfileEditPage
    {
        private StudentInfoDto _studentFullProfile;

        private string _githubUsername;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _studentFullProfile = await ClientHolder.Student.GetSelf();
            _githubUsername = _studentFullProfile.GithubUsername;
        }

        private async Task OnSave()
        {
            if (_githubUsername is not null && _studentFullProfile.GithubUsername != _githubUsername)
            {
                await ClientHolder.Student.Update(new StudentUpdateRequestDto(_githubUsername));
            }
            else
            {
                //TODO: alert?
            }
        }
    }
}
