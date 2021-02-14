using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Assignments
{
    public partial class AssignmentPage
    {
        private ICollection<AssignmentInfoDto> _assignment;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _assignment = await ClientHolder.ApiAssignmentsGetAsync();
        }

        private async Task MakeCompleted(int assignmentId)
        {
            await ClientHolder.ApiAssignmentsCompleteAsync(assignmentId);
            _assignment = await ClientHolder.ApiAssignmentsGetAsync();
        }

        private async Task MakeUncompleted(int assignmentId)
        {
            await ClientHolder.ApiAssignmentsUndoAsync(assignmentId);
            _assignment = await ClientHolder.ApiAssignmentsGetAsync();
        }

        private async Task Delete(int assignmentId)
        {
            await ClientHolder.ApiAssignmentsDeleteAsync(assignmentId);
            _assignment = await ClientHolder.ApiAssignmentsGetAsync();
        }
    }
}
