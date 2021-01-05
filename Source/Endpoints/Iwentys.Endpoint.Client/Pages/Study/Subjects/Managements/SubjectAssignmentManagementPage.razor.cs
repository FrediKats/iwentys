using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Managements
{
    public partial class SubjectAssignmentManagementPage
    {
        private List<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //TODO: ensure user is teacher for this subject
            _subjectAssignments = await ClientHolder.SubjectAssignment.GetSubjectAssignmentForSubject(SubjectId);
        }

        private string LinkToSubjectAssignmentCreate() => $"/subject/{SubjectId}/management/assignments/create";
        private string LinkToSubjectAssignmentSubmitJournal() => $"/subject/{SubjectId}/management/assignments/submits";
    }
}
