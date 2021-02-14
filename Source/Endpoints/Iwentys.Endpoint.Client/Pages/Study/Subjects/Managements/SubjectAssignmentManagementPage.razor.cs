using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Managements
{
    public partial class SubjectAssignmentManagementPage
    {
        private ICollection<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignments = await ClientHolder.ApiSubjectAssignmentForSubjectAsync(SubjectId);
        }

        private string LinkToSubjectAssignmentCreate() => $"/subject/{SubjectId}/management/assignments/create";
        private string LinkToSubjectAssignmentSubmitJournal() => $"/subject/{SubjectId}/management/assignments/submits";
    }
}
