﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentManagementPage
    {
        private ICollection<SubjectAssignmentDto> _subjectAssignments;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignments = await SubjectAssignmentClient.GetAvailableSubjectAssignmentsAsync();
            //TODO: group by subject
        }

        //private string LinkToSubjectAssignmentCreate() => $"/subject/{SubjectId}/management/assignments/create";
        //private string LinkToSubjectAssignmentSubmitJournal() => $"/subject/{SubjectId}/management/assignments/submits";
    }
}