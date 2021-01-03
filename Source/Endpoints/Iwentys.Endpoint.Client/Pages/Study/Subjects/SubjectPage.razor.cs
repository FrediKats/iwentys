using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
{
    public partial class SubjectPage
    {
        private SubjectProfileDto _subjectProfile;
        private List<NewsfeedViewModel> _newsfeeds;
        private List<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectProfile = await ClientHolder.Subject.GetProfile(SubjectId);
            _newsfeeds = await ClientHolder.Newsfeed.GetForSubject(SubjectId);
            _subjectAssignments = await ClientHolder.SubjectAssignment.GetSubjectAssignmentForSubject(SubjectId);
        }
        
        private string LinkToCreateNewsfeedPage() => $"/newsfeed/create-subject/{SubjectId}";
        private string LinkToSubjectAssignmentManagement() => $"/subject/{SubjectId}/management/assignments";
        private string LinkToMyAssignmentManagement() => $"/subject/{SubjectId}/assignments";
    }
}
