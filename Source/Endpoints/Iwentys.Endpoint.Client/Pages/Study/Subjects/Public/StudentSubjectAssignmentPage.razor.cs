using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Students.Models;
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
            //TODO: ensure user is teacher for this subject
            _subjectAssignments = await ClientHolder.SubjectAssignment.GetSubjectAssignmentForSubject(SubjectId);
            _subjectAssignmentSubmits = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmits(SubjectId, _self.Id);
        }
    }
}
