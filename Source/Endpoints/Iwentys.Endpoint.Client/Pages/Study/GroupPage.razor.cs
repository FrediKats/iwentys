using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Models.Transferable.Study;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class GroupPage : ComponentBase
    {
        private GroupProfileResponse _groupProfile;

        protected override async Task OnInitializedAsync()
        {
            var studentControllerClient = new StudyGroupControllerClient(await Http.TrySetHeader(LocalStorage));

            _groupProfile = await studentControllerClient.Get(GroupName);
        }
    }
}
