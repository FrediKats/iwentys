using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentSubmitJounalPage
    {
        private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await _subjectAssignmentSubmitClient.SearchSubjectAssignmentSubmitsAsync(new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = SubjectId
            });
        }

        private void NavigateToSubmitPage(object row)
        {
            //TODO: replace exception
            if (row is not SubjectAssignmentSubmitDto submit)
                throw new Exception("Something goes wrong.");

            _navigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments/submits/{submit.Id}");
        }
    }
}
