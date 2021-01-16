using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.Models.Students;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Public
{
    public partial class StudentSubjectAssignmentPage
    {
        private StudentInfoDto _self;
        private List<SubjectAssignmentDto> _subjectAssignments;
        private List<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _self = await ClientHolder.Student.GetSelf();
            _subjectAssignments = await ClientHolder.SubjectAssignment.GetSubjectAssignmentForSubject(SubjectId);
            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.GetStudentSubjectAssignmentSubmits(SubjectId);
        }

        private string LinkToCreateSubmit() => $"/subject/{SubjectId}/assignments/create-submit";
    }
}
