using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Models.Transferable.Study;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class SubjectPage : ComponentBase
    {
        private SubjectProfileResponse _subjectProfile;

        protected override async Task OnInitializedAsync()
        {
            var studentControllerClient = new SubjectControllerClient(await Http.TrySetHeader(LocalStorage));

            _subjectProfile = await studentControllerClient.GetProfile(SubjectId);
        }
    }
}
