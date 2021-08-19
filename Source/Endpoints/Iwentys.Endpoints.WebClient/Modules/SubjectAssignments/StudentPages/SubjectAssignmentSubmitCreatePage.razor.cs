using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.StudentPages
{
    public partial class SubjectAssignmentSubmitCreatePage
    {
        public class Arguments
        {
            [Required]
            public string Description { get; set; }
            [Required]
            public SubjectAssignmentDto SelectedSubjectAssignment { get; set; }
            [Required]
            public string Link { get; set; }
        }
        
        private List<SubjectAssignmentDto> _subjectAssignments;
        private Arguments _arguments = new Arguments();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _subjectAssignments = (await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync(SubjectId)).ToList();
        }

        public async Task SendSubmit()
        {
            var createArguments = new SubjectAssignmentSubmitCreateArguments
            {
                SubjectAssignmentId = _arguments.SelectedSubjectAssignment.Id,
                StudentDescription = _arguments.Description,
                StudentPRLink = _arguments.Link
            };

            await _studentSubjectAssignmentSubmitClient.CreateSubmitAsync(createArguments);
            _navigationManager.NavigateTo($"/subject/{SubjectId}/assignments");
        }
    }
}
