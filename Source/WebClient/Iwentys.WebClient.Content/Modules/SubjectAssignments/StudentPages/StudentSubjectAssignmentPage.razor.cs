using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class StudentSubjectAssignmentPage
{
    private enum ViewMode
    {
        All,
        Completed,
        Uncompleted
    }
        
    private ICollection<SubjectAssignmentDto> _subjectAssignments;
    private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;
    private ViewMode _currentViewMode = ViewMode.Uncompleted;
    private StudentInfoDto _self;
        
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _self = await _studentClient.GetSelfAsync();
        _subjectAssignments = await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync(SubjectId);
        _subjectAssignmentSubmits = await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentSubmitsAsync(SubjectId);
    }

    private bool SubjectAssignmentSatisfyViewMode(SubjectAssignmentDto subjectAssignment)
    {
        return _currentViewMode switch
        {
            ViewMode.Completed => ApprovedSubmitByCurrentUserExists(subjectAssignment),
            ViewMode.Uncompleted => !ApprovedSubmitByCurrentUserExists(subjectAssignment),
            ViewMode.All => true
        };
    }

    private bool ApprovedSubmitByCurrentUserExists(SubjectAssignmentDto subjectAssignment)
    {
        return subjectAssignment.Submits.Any(
            submit => submit.Student.Id == _self.Id && (submit.State == SubmitState.Approved));
    }
        
    private string LinkToCreateSubmit() => $"/subject/{SubjectId}/assignments/create-submit";
}