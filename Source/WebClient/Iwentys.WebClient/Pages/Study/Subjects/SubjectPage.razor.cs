﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Pages.Study.Subjects
{
    public partial class SubjectPage
    {
        private SubjectProfileDto _subjectProfile;
        private ICollection<NewsfeedViewModel> _newsfeeds;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectProfile = await SubjectClient.GetSubjectByIdAsync(SubjectId);
            _newsfeeds = await NewsfeedClient.GetBySubjectIdAsync(SubjectId);
        }
        
        private string LinkToCreateNewsfeedPage() => $"/newsfeed/create-subject/{SubjectId}";
        private string LinkToSubjectAssignmentManagement() => $"/subject/{SubjectId}/management/assignments";
        private string LinkToMyAssignmentManagement() => $"/subject/{SubjectId}/assignments";
    }
}
