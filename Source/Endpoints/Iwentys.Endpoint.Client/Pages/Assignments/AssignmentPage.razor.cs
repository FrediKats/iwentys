using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Features.Assignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Assignments
{
    public partial class AssignmentPage
    {
        private AssignmentControllerClient _assignmentControllerClient;

        private List<AssignmentInfoDto> _assignment;

        protected override async Task OnInitializedAsync()
        {
            _assignmentControllerClient = new AssignmentControllerClient(await Http.TrySetHeader(LocalStorage));
            _assignment = await _assignmentControllerClient.Get();
        }

        private async Task MakeCompleted(int assignmentId)
        {
            await _assignmentControllerClient.Complete(assignmentId);
            _assignment = await _assignmentControllerClient.Get();
        }

        private async Task MakeUncompleted(int assignmentId)
        {
            await _assignmentControllerClient.Undo(assignmentId);
            _assignment = await _assignmentControllerClient.Get();
        }

        private async Task Delete(int assignmentId)
        {
            await _assignmentControllerClient.Delete(assignmentId);
            _assignment = await _assignmentControllerClient.Get();
        }
    }
}
