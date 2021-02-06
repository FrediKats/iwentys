using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
{
    public partial class SubjectPage
    {
        private SubjectProfileDto _subjectProfile;
        private ICollection<NewsfeedViewModel> _newsfeeds;
        private ICollection<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectProfile = await ClientHolder.ApiSubjectProfileAsync(SubjectId);
            _newsfeeds = await ClientHolder.ApiNewsfeedSubjectGetAsync(SubjectId);
            _subjectAssignments = await ClientHolder.ApiSubjectAssignmentForSubjectAsync(SubjectId);
        }
        
        private string LinkToCreateNewsfeedPage() => $"/newsfeed/create-subject/{SubjectId}";
        private string LinkToSubjectAssignmentManagement() => $"/subject/{SubjectId}/management/assignments";
        private string LinkToMyAssignmentManagement() => $"/subject/{SubjectId}/assignments";
    }
}
