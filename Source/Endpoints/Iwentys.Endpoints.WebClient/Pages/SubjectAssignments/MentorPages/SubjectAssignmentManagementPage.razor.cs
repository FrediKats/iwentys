using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentManagementPage
    {
        private ICollection<SubjectAssignmentJournalItemDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            _subjectAssignments = await _subjectAssignmentClient.GetAvailableSubjectAssignmentsAsync();
        }

        private string LinkToSubjectAssignmentCreate(int subjectId) => $"/subject/assignment-management/{subjectId}/create";
        //private string LinkToSubjectAssignmentSubmitJournal() => $"/subject/{SubjectId}/management/assignments/submits";
    }
}
