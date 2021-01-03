using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
{
    public partial class SubjectAssignmentSubmitJounalPage
    {
        private List<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmits(SubjectId);
        }
    }
}
