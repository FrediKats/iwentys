using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class SubjectPage
    {
        private SubjectProfileDto _subjectProfile;
        private List<NewsfeedViewModel> _newsfeeds;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _subjectProfile = await ClientHolder.Subject.GetProfile(SubjectId);
            _newsfeeds = await ClientHolder.Newsfeed.GetForSubject(SubjectId);
        }
        
        private string LinkToCreateNewsfeedPage() => $"/newsfeed/create-subject/{SubjectId}";
    }
}
