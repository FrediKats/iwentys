using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Public
{
    public partial class SubjectAssignmentSubmitCreatePage
    {
        private StudentInfoDto _self;
        private ICollection<SubjectAssignmentDto> _subjectAssignments;

        private string _description;
        private SubjectAssignmentDto _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _self = await ClientHolder.ApiStudentSelfAsync();
            _subjectAssignments = await ClientHolder.ApiSubjectAssignmentForSubjectAsync(SubjectId);
        }

        public async Task SendSubmit()
        {
            var createArguments = new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _description
            };

            SubjectAssignmentSubmitDto submit = await ClientHolder.ApiSubjectAssignmentAssignmentsSubmitsAsync(SubjectId, _selectedSubject.Id, createArguments);

        }
    }
}
