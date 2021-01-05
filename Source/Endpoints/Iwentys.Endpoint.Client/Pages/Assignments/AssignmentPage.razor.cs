using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Assignments
{
    public partial class AssignmentPage
    {
        private List<AssignmentInfoDto> _assignment;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _assignment = await ClientHolder.Assignment.Get();
        }

        private async Task MakeCompleted(int assignmentId)
        {
            await ClientHolder.Assignment.Complete(assignmentId);
            _assignment = await ClientHolder.Assignment.Get();
        }

        private async Task MakeUncompleted(int assignmentId)
        {
            await ClientHolder.Assignment.Undo(assignmentId);
            _assignment = await ClientHolder.Assignment.Get();
        }

        private async Task Delete(int assignmentId)
        {
            await ClientHolder.Assignment.Delete(assignmentId);
            _assignment = await ClientHolder.Assignment.Get();
        }
    }
}
