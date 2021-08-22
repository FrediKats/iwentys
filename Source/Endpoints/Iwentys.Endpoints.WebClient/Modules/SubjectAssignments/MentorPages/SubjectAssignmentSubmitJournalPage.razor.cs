using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentSubmitJournalPage
    {
        private IEnumerable<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;
        private IEnumerable<SubjectAssignmentSubmitDto> _tableSubjectAssignmentSubmits;
        private string _searchString = "";
        private string _stateSelectorValue = "State";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subjectAssignmentSubmits = await _mentorSubjectAssignmentSubmitClient.SearchSubjectAssignmentSubmitsAsync(new SubjectAssignmentSubmitSearchArguments
            {
                SubjectId = SubjectId
            });
            _tableSubjectAssignmentSubmits = new List<SubjectAssignmentSubmitDto>(_subjectAssignmentSubmits);
        }

        private void NavigateToSubmitPage(object row)
        {
            //TODO: replace exception
            if (row is not SubjectAssignmentSubmitDto submit)
                throw new Exception("Something goes wrong.");

            _navigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments/submits/{submit.Id}");
        }

        private bool IsMatchedWithSearchRequest(SubjectAssignmentSubmitDto assignment)
        {
            //SearchString
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            if (assignment.SubjectAssignmentTitle.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (assignment.Student.SecondName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (assignment.Student.FirstName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{assignment.SubmitTimeUtc} {assignment.RejectTimeUtc} {assignment.ApproveTimeUtc}".Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //Filter
            Console.WriteLine(assignment.State.ToString() == _stateSelectorValue);
            return false;
        }
        
    }
}
