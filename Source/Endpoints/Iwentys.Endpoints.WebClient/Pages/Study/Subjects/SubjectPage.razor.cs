using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Study.Subjects
{
    public partial class SubjectPage
    {
        private SubjectProfileDto _subjectProfile;
        private ICollection<NewsfeedViewModel> _newsfeeds;
        private ICollection<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectProfile = await SubjectClient.GetSubjectByIdAsync(SubjectId);
            _newsfeeds = await NewsfeedClient.GetBySubjectIdAsync(SubjectId);
            _subjectAssignments = await SubjectAssignmentClient.GetBySubjectIdAsync(SubjectId);
        }
        
        private string LinkToCreateNewsfeedPage() => $"/newsfeed/create-subject/{SubjectId}";
        private string LinkToSubjectAssignmentManagement() => $"/subject/{SubjectId}/management/assignments";
        private string LinkToMyAssignmentManagement() => $"/subject/{SubjectId}/assignments";
    }
}
