using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class SubjectPage
{
    private SubjectProfileDto _subjectProfile;
    private ICollection<NewsfeedViewModel> _newsfeeds;

    protected override async Task OnInitializedAsync()
    {
        _subjectProfile = await _subjectClient.GetSubjectByIdAsync(SubjectId);
        _newsfeeds = await _newsfeedClient.GetBySubjectIdAsync(SubjectId);
    }
        
    private string LinkToCreateNewsfeedPage() => $"/newsfeed/create-subject/{SubjectId}";
    private string LinkToSubjectAssignmentManagement() => $"/subject/{SubjectId}/management/assignments";
    private string LinkToMyAssignmentManagement() => $"/subject/{SubjectId}/assignments";
}