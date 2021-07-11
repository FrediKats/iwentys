using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages.Components
{
    public partial class SubjectAssignmentComponent
    {
        private string LinkToSubjectAssignmentCreate(int subjectId) => $"/subject/assignment-management/{subjectId}/create";
        private string LinkToSubjectAssignmentUpdate(int subjectId) => $"/subject/assignment-management/{subjectId}/update";
        private string LinkToSubjectAssignmentSubmitJournal(int subjectId) => $"/subject/assignment-management/{subjectId}/submits";

        private async Task Delete(SubjectAssignmentDto assignmentDto)
        {
            await _mentorSubjectAssignmentClient.DeleteAsync(assignmentDto.Id);
            assignmentDto.AvailabilityState = AvailabilityState.Deleted;
        }

        private async Task Recover(SubjectAssignmentDto assignmentDto)
        {
            await _mentorSubjectAssignmentClient.RecoverAsync(assignmentDto.Id);
            assignmentDto.AvailabilityState = AvailabilityState.Visible;
        }
    }
}
