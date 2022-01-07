using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.Assignments
{
    public partial class AssignmentPage
    {
        private ICollection<AssignmentInfoDto> _assignment;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _assignment = await AssignmentClient.GetAsync();
        }

        private async Task MakeCompleted(int assignmentId)
        {
            await AssignmentClient.CompleteAsync(assignmentId);
            _assignment = await AssignmentClient.GetAsync();
        }

        private async Task MakeUncompleted(int assignmentId)
        {
            await AssignmentClient.UndoAsync(assignmentId);
            _assignment = await AssignmentClient.GetAsync();
        }

        private async Task Delete(int assignmentId)
        {
            await AssignmentClient.DeleteAsync(assignmentId);
            _assignment = await AssignmentClient.GetAsync();
        }
    }
}
