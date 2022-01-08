using Iwentys.Sdk;

namespace Iwentys.WebClient.Content.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentManagementPage
    {
        private ICollection<SubjectAssignmentJournalItemDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            _subjectAssignments = await _mentorSubjectAssignmentClient.GetMentorSubjectAssignmentsAsync();
        }
    }
}
