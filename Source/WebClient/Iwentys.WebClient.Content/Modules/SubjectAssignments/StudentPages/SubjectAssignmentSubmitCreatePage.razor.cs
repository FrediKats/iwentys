using System.ComponentModel.DataAnnotations;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

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
        _subjectAssignments = Enumerable.ToList<SubjectAssignmentDto>((await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync(SubjectId)));
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