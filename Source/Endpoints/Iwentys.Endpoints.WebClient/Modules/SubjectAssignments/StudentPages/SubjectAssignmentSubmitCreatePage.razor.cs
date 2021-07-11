using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.StudentPages
{
    public partial class SubjectAssignmentSubmitCreatePage
    {
        private List<SubjectAssignmentDto> _subjectAssignments;

        private string _description;
        private SubjectAssignmentDto _selectedSubjectAssignment;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _subjectAssignments = (await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync(SubjectId)).ToList();
        }

        public async Task SendSubmit()
        {
            var createArguments = new SubjectAssignmentSubmitCreateArguments
            {
                SubjectAssignmentId = _selectedSubjectAssignment.Id,
                StudentDescription = _description,
            };

            await _studentSubjectAssignmentSubmitClient.CreateSubmitAsync(createArguments);
            _navigationManager.NavigateTo($"/subject/{SubjectId}/assignments");
        }
    }
}
