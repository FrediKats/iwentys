using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class StudentSubjectAssignmentJournalPage
{
    private GroupProfileResponseDto _group;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        StudentInfoDto currentStudent = await _studentClient.GetSelfAsync();
        _group = await _studyGroupClient.GetByStudentIdAsync(currentStudent.Id);
    }

    private string LinkToMyAssignmentManagement(int subjectId) => $"/subject/{subjectId}/assignments";
}