using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.SubjectAssignments.StudentPages
{
    public partial class StudentSubjectAssignmentPage
    {
        private StudentInfoDto _self;
        private ICollection<SubjectAssignmentJournalItemDto> _subjectAssignments;
        private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _self = await StudentClient.GetSelfAsync();
            _subjectAssignments = await SubjectAssignmentClient.GetAvailableSubjectAssignmentsAsync();
            _subjectAssignmentSubmits = await SubjectAssignmentSubmitClient.SearchSubjectAssignmentSubmitsAsync(new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = SubjectId
            });
        }

        private string LinkToCreateSubmit() => $"/subject/{SubjectId}/assignments/create-submit";
    }
}
