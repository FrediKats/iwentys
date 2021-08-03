using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentSubmitJournalPage
    {
        private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;

        private string _searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await _mentorSubjectAssignmentSubmitClient.SearchSubjectAssignmentSubmitsAsync(new SubjectAssignmentSubmitSearchArguments
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

        private bool IsMatchedWithSearchRequest(SubjectAssignmentSubmitDto student)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (student.SubjectAssignmentTitle.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (student.Student.SecondName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (student.Student.FirstName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{student.SubmitTimeUtc} {student.RejectTimeUtc} {student.ApproveTimeUtc}".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}
