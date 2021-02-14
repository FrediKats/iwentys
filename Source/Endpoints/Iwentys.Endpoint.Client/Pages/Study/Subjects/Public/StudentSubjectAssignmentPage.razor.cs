using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Public
{
    public partial class StudentSubjectAssignmentPage
    {
        private StudentInfoDto _self;
        private ICollection<SubjectAssignmentDto> _subjectAssignments;
        private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _self = await ClientHolder.ApiStudentSelfAsync();
            _subjectAssignments = await ClientHolder.ApiSubjectAssignmentForSubjectAsync(SubjectId);
            _subjectAssignmentSubmits = await ClientHolder.ApiSubjectAssignmentSubmitsGetAsync(SubjectId);
        }

        private string LinkToCreateSubmit() => $"/subject/{SubjectId}/assignments/create-submit";
    }
}
