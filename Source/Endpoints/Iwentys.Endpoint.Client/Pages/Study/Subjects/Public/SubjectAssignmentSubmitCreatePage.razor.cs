using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Study.Models.Students;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Public
{
    public partial class SubjectAssignmentSubmitCreatePage
    {
        private StudentInfoDto _self;
        private List<SubjectAssignmentDto> _subjectAssignments;

        private string _description;
        private SubjectAssignmentDto _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _self = await ClientHolder.Student.GetSelf();
            _subjectAssignments = await ClientHolder.SubjectAssignment.GetSubjectAssignmentForSubject(SubjectId);
        }

        public async Task SendSubmit()
        {
            var createArguments = new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _description
            };

            SubjectAssignmentSubmitDto submit = await ClientHolder.SubjectAssignment.SendSubmit(SubjectId, _selectedSubject.Id, createArguments);

        }
    }
}
