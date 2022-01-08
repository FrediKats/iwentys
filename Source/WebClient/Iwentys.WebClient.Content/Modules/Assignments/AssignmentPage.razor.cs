using Iwentys.Sdk;

namespace Iwentys.WebClient.Content
{
    public partial class AssignmentPage
    {
        private ICollection<AssignmentInfoDto> _assignment;

        protected override async Task OnInitializedAsync()
        {
            _assignment = await _assignmentClient.GetAsync();
        }

        private async Task MakeCompleted(int assignmentId)
        {
            await _assignmentClient.CompleteAsync(assignmentId);
            _assignment = await _assignmentClient.GetAsync();
        }

        private async Task MakeUncompleted(int assignmentId)
        {
            await _assignmentClient.UndoAsync(assignmentId);
            _assignment = await _assignmentClient.GetAsync();
        }

        private async Task Delete(int assignmentId)
        {
            await _assignmentClient.DeleteAsync(assignmentId);
            _assignment = await _assignmentClient.GetAsync();
        }
    }
}
