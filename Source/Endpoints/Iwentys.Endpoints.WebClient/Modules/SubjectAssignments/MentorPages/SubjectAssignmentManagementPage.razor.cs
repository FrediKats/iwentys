using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
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
