using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
{
    public partial class SubjectAssignmentManagementPage
    {
        private SubjectProfileDto _subjectProfile;
        private List<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //TODO: ensure user is teacher for this subject
            _subjectProfile = await ClientHolder.Subject.GetProfile(SubjectId);
            _subjectAssignments = await ClientHolder.SubjectAssignment.GetSubjectAssignmentForSubject(SubjectId);
        }

        private string LinkToSubjectAssignmentCreate() => $"/subject/{SubjectId}/assignment/create";
        private string LinkToSubjectAssignmentSubmitJournal() => $"/subject/{SubjectId}/assignments/submits";
    }
}
